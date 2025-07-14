using Microsoft.VisualBasic;
using Microsoft.Win32; // 使用新的对话框命名空间
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel; // 需要这个命名空间
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VoaDownloaderWpf
{
    public class MainViewModel : BaseViewModel
    {
        private readonly VoaScraperService _scraperService;
        private ArticleViewModel _selectedArticle; // 新增：用于追踪当前选中的文章
        private Dictionary<string, string> _categoryUrlMap = new Dictionary<string, string>();

        // --- Backing Fields ---
        private string _statusText = "就绪";
        private double _progressValue;
        private string _selectedCategory;
        private int _pageNumber = 1;
        private string _previewContent;
        private bool _isBusy;
        // 修改：isSelectAll 不再需要手动设置字段
        private bool? _isSelectAll = false;

        // --- Properties for UI Binding ---
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();
        public ObservableCollection<ArticleViewModel> Articles { get; } = new ObservableCollection<ArticleViewModel>();

        public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(); } }
        public double ProgressValue { get => _progressValue; set { _progressValue = value; OnPropertyChanged(); } }
        public string SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; OnPropertyChanged(); } }
        public int PageNumber { get => _pageNumber; set { _pageNumber = value; OnPropertyChanged(); } }
        public string PreviewContent { get => _previewContent; set { _previewContent = value; OnPropertyChanged(); } }
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }

        // ########## 改进 1：双向“全选”功能的实现 ##########
        public bool? IsSelectAll
        {
            get => _isSelectAll;
            set
            {
                // 只在值改变时（从 true/false -> null, 或从 null -> true/false）或被用户点击时才执行
                if (value.HasValue && _isSelectAll != value)
                {
                    _isSelectAll = value;
                    SelectAllArticles(value.Value);
                    OnPropertyChanged();
                }
            }
        }

        // --- Commands ---
        public ICommand FetchArticlesCommand { get; }
        public ICommand DownloadCommand { get; }
        public ICommand ArticleSelectionChangedCommand { get; }
        public ICommand IncrementPageCommand { get; } // 新增：增加页码命令
        public ICommand DecrementPageCommand { get; } // 新增：减少页码命令
        public ICommand OpenReaderCommand { get; } // 新增：打开阅读器命令
        // 【新增】打开生词本的命令
        public ICommand OpenVocabBookCommand { get; }

        public MainViewModel()
        {
            _scraperService = new VoaScraperService();

            FetchArticlesCommand = new RelayCommand(async _ => await FetchArticlesAsync(), _ => !IsBusy && !string.IsNullOrEmpty(SelectedCategory));
            DownloadCommand = new RelayCommand(async _ => await DownloadAsync(), _ => !IsBusy && Articles.Any(a => a.IsSelected));
            ArticleSelectionChangedCommand = new RelayCommand(async param => await OnArticleSelectedAsync(param as ArticleViewModel));
            // --- 新增：初始化页码控制命令 ---
            IncrementPageCommand = new RelayCommand(_ => PageNumber++, _ => !IsBusy);
            DecrementPageCommand = new RelayCommand(_ => PageNumber--, _ => !IsBusy && PageNumber > 1);
            // 新增：初始化打开阅读器命令
            OpenReaderCommand = new RelayCommand(async _ => await OpenReaderWindowAsync(), _ => Articles.Any(a => a.IsSelected));
            // 【新增】初始化新命令
            OpenVocabBookCommand = new RelayCommand(_ => OpenVocabBookWindow());

            // ------------------------------------

            // ########## 优化 1：改进 ViewModel 的异步加载方式 ##########
            // 在构造函数中调用一个 async void 方法
            LoadInitialDataAsync();
        }

        // 优化 1：创建一个 async void 的加载方法
        private async void LoadInitialDataAsync()
        {
            IsBusy = true;
            StatusText = "正在加载分类...";
            try
            {
                _categoryUrlMap = await _scraperService.LoadCategoriesAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Categories.Clear();
                    foreach (var name in _categoryUrlMap.Keys)
                    {
                        Categories.Add(name);
                    }
                    if (Categories.Any())
                    {
                        SelectedCategory = Categories.First();
                    }
                });
                StatusText = "分类加载完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载分类失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText = "加载分类失败";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // 用这个新版本替换整个 OpenReaderWindowAsync 方法
        private async Task OpenReaderWindowAsync()
        {
            // 1. 获取所有被选中的文章
            var selectedArticles = Articles.Where(a => a.IsSelected).ToList();
            if (!selectedArticles.Any())
            {
                MessageBox.Show("请至少选择一篇文章来开始阅读。", "提示", MessageBoxButton.OK);
                return;
            }

            IsBusy = true;
            StatusText = "正在准备播放列表...";
            try
            {
                // 2. 创建 ReadingViewModel，并传入整个列表和 ScraperService 实例
                // ReadingViewModel 现在需要 scraper service 来独立加载文章数据
                var readingViewModel = new ReadingViewModel(selectedArticles, _scraperService);

                var readingWindow = new ReadingWindow
                {
                    DataContext = readingViewModel
                };
                readingWindow.Show();
                StatusText = "阅读器已打开";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开阅读器失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task FetchArticlesAsync()
        {
            IsBusy = true;
            StatusText = "正在获取文章列表...";
            ProgressValue = 0;
            PreviewContent = "";

            // 获取文章前，先注销旧列表的事件监听，防止内存泄漏
            foreach (var oldArticle in Articles)
            {
                oldArticle.PropertyChanged -= Article_PropertyChanged;
            }
            Application.Current.Dispatcher.Invoke(() => Articles.Clear());

            try
            {
                var categoryUrl = _categoryUrlMap[SelectedCategory];
                var articlesData = await _scraperService.FetchArticlesAsync(categoryUrl, PageNumber);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var entry in articlesData)
                    {
                        var articleVm = new ArticleViewModel { Title = entry.Key, Url = entry.Value };
                        // 改进 1：为每篇文章添加属性变化监听
                        articleVm.PropertyChanged += Article_PropertyChanged;
                        Articles.Add(articleVm);
                    }
                });
                UpdateSelectAllState(); // 更新“全选”状态
                StatusText = $"获取到 {Articles.Count} 篇文章";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取文章列表失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText = "获取文章失败";
            }
            finally
            {
                IsBusy = false;
            }
        }
        // 【新增】打开生词本窗口的方法
        private void OpenVocabBookWindow()
        {
            var vocabViewModel = new VocabBookViewModel();
            var vocabWindow = new VocabBookWindow
            {
                DataContext = vocabViewModel
            };
            // 使用 Show() 而不是 ShowDialog()，允许用户同时操作主窗口和生词本窗口
            vocabWindow.Show();
        }
        private async Task OnArticleSelectedAsync(ArticleViewModel article)
        {
            if (article == null) return;
            _selectedArticle = article; // 更新当前选中的文章
                                        // (CommandManager as an alternative) Invalidate the command to re-evaluate CanExecute
            (OpenReaderCommand as RelayCommand)?.RaiseCanExecuteChanged();

            PreviewContent = "正在加载预览...";
            try
            {
                var (content, _) = await _scraperService.GetArticleDetailsAsync(article.Url);
                PreviewContent = content ?? "无法加载预览内容。";
            }
            catch (Exception ex)
            {
                PreviewContent = $"加载预览失败: {ex.Message}";
            }
        }

        private void SelectAllArticles(bool select)
        {
            // 临时移除监听，避免循环更新
            foreach (var article in Articles) article.PropertyChanged -= Article_PropertyChanged;

            foreach (var article in Articles)
            {
                article.IsSelected = select;
            }

            // 操作完成后，重新添加监听
            foreach (var article in Articles) article.PropertyChanged += Article_PropertyChanged;
        }

        // 改进 1：监听文章的 IsSelected 属性变化
        private void Article_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ArticleViewModel.IsSelected))
            {
                UpdateSelectAllState();
            }
        }

        // 改进 1：根据所有文章的选中状态，更新“全选”复选框的状态
        private void UpdateSelectAllState()
        {
            int selectedCount = Articles.Count(a => a.IsSelected);
            if (selectedCount == Articles.Count && Articles.Count > 0)
            {
                _isSelectAll = true;
            }
            else if (selectedCount == 0)
            {
                _isSelectAll = false;
            }
            else
            {
                // 当部分选中时，设置为中间状态 (null)
                _isSelectAll = null;
            }
            OnPropertyChanged(nameof(IsSelectAll));
        }


        private async Task DownloadAsync()
        {
            var itemsToDownload = Articles.Where(a => a.IsSelected).ToList();
            if (!itemsToDownload.Any())
            {
                MessageBox.Show("请至少选择一篇文章进行下载。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // ########## 改进 2：使用 WPF 原生文件夹选择对话框 ##########
            var dialog = new OpenFolderDialog
            {
                Title = "请选择一个用于保存所有下载内容的根文件夹"
            };

            if (dialog.ShowDialog() != true) return;
            // ######################################################

            IsBusy = true;
            var baseSavePath = dialog.FolderName;
            var failedDownloads = new List<string>();

            for (int i = 0; i < itemsToDownload.Count; i++)
            {
                var article = itemsToDownload[i];
                StatusText = $"({i + 1}/{itemsToDownload.Count}) 正在下载: {article.Title}";
                ProgressValue = 0;
                var progress = new Progress<double>(value => ProgressValue = value);

                try
                {
                    var (content, audioUrl) = await _scraperService.GetArticleDetailsAsync(article.Url);
                    if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(audioUrl))
                    {
                        failedDownloads.Add(article.Title);
                        continue;
                    }

                    string sanitizedTitle = _scraperService.MakeValidFileName(article.Title);
                    string articleFolderPath = Path.Combine(baseSavePath, sanitizedTitle);
                    Directory.CreateDirectory(articleFolderPath);

                    string textPath = Path.Combine(articleFolderPath, $"{sanitizedTitle}.txt");
                    string audioPath = Path.Combine(articleFolderPath, $"{sanitizedTitle}.mp3");

                    await File.WriteAllTextAsync(textPath, content, Encoding.UTF8);
                    await _scraperService.DownloadFileWithProgressAsync(audioUrl, audioPath, article.Url, progress);
                }
                catch
                {
                    failedDownloads.Add(article.Title);
                }
            }

            var report = new StringBuilder();
            report.AppendLine($"批量下载完成！共 {itemsToDownload.Count - failedDownloads.Count} 篇成功。");
            if (failedDownloads.Any())
            {
                report.AppendLine("\n以下文章下载失败:");
                foreach (var failed in failedDownloads) report.AppendLine($"- {failed}");
            }
            MessageBox.Show(report.ToString(), "下载报告", MessageBoxButton.OK, MessageBoxImage.Information);

            StatusText = "就绪";
            ProgressValue = 0;
            IsBusy = false;
        }
    }
}
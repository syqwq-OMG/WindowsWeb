using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO; // 需要此 using
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace VoaDownloaderWpf
{
    public class SpeedRatioOption
    {
        public string DisplayName { get; set; }
        public double Value { get; set; }
    }
    public class ReadingViewModel : BaseViewModel
    {
        #region Playback Speed Properties and Commands
        // 【新增】倍速播放相关
        public List<SpeedRatioOption> AvailableSpeedRatios { get; private set; }
        private double _currentSpeedRatio = 1.0;
        public double CurrentSpeedRatio
        {
            get => _currentSpeedRatio;
            set { _currentSpeedRatio = value; OnPropertyChanged(); }
        }
        public ICommand SetSpeedCommand { get; private set; }
        #endregion

        #region Seeking Properties and Commands
        // 【新增】进度条拖动相关
        public ICommand SliderDragStartedCommand { get; private set; }
        public ICommand SliderDragCompletedCommand { get; private set; }
        #endregion

        // --- 依赖与状态 ---
        private readonly VoaScraperService _scraperService;
        private readonly List<ArticleViewModel> _playlist;
        private int _currentIndex = -1;
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isUserDraggingSlider = false;
        private IReadingView _view; // 新增：持有对视图接口的引用
        private string _currentAudioUrl; // 【新增】用于存储当前文章的音频URL
        private ArticleViewModel _currentArticle; // 【新增】用于存储当前文章的信息
        // 【新增】用于从本地笔记加载的FlowDocument
        public FlowDocument PreloadedDocument { get; private set; }
        // 【新增】用于区分模式的字段
        private readonly bool _isOfflineMode;
        private readonly string _localNoteFolderPath; // 仅在离线模式下有值

        // --- Backing Fields ---
        private string _articleTitle;
        private string _articleContent;
        private double _currentPosition;
        private double _maximumPosition = 1;
        private bool _isPlaying = false;
        private bool _isBusy = false; // 用于加载状态
        private string _statusText;
        // 【新增字段】用于持有AI服务实例
        private GeminiAiService _aiService;
        // 【新增】用于存储上一次“正在播放”时的状态文本
        private string _lastPlayingStatusText;


        // --- UI 绑定属性 ---
        public string ArticleTitle { get => _articleTitle; set { _articleTitle = value; OnPropertyChanged(); } }
        public string ArticleContent { get => _articleContent; set { _articleContent = value; OnPropertyChanged(); } }
        public double CurrentPosition { get => _currentPosition; set { _currentPosition = value; OnPropertyChanged(); } }
        public double MaximumPosition { get => _maximumPosition; set { _maximumPosition = value; OnPropertyChanged(); } }
        public bool IsPlaying { get => _isPlaying; set { _isPlaying = value; OnPropertyChanged(); } }
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }
        public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(); } }
        private string _selectedWordDefinition = "在左侧选择单词以查看释义。";
        public string SelectedWordDefinition
        {
            get => _selectedWordDefinition;
            private set { _selectedWordDefinition = value; OnPropertyChanged(); }
        }
        // 【新增】用于显示原单词的属性
        private string _selectedWord;
        public string SelectedWord
        {
            get => _selectedWord;
            private set { _selectedWord = value; OnPropertyChanged(); }
        }
        // 判断“加入生词本”按钮是否可用的标志
        private bool _canAddToVocab;
        public bool CanAddToVocab
        {
            get => _canAddToVocab;
            set { _canAddToVocab = value; OnPropertyChanged(); }
        }

        // 【核心修正】在声明时就初始化ChatHistory，确保它永远不为null
        public ObservableCollection<ChatMessage> ChatHistory { get; } = new ObservableCollection<ChatMessage>();
        private string _userInput;
        public string UserInput { get => _userInput; set { _userInput = value; OnPropertyChanged(); } }
        private bool _isAiThinking;
        public bool IsAiThinking { get => _isAiThinking; set { _isAiThinking = value; OnPropertyChanged(); } }


        // --- 命令 ---
        public ICommand PlayPauseCommand { get; private set; }
        public ICommand NextArticleCommand { get; }
        public ICommand PreviousArticleCommand { get; }
        public ICommand WindowClosingCommand { get; private set; }
        // 【全新】定义与UI绑定的命令
        public ICommand HighlightCommand { get; private set; }
        public ICommand UnderlineCommand { get; private set; }
        public ICommand EraserCommand { get; private set; }
        // 【新增】查看生词本命令
        public ICommand ViewVocabBookCommand { get; private set; }
        // 【新增】加入生词本命令
        public ICommand AddToVocabCommand { get; private set; }
        // 【新增】AI聊天相关命令
        public ICommand SendMessageCommand { get; private set; }
        // 【新增】导出笔记命令
        public ICommand ExportNotesCommand { get; private set; }
        // 【新增】新的保存命令
        public ICommand SaveOrUpdateNotesCommand { get; private set; }
        public ICommand AddToPhraseBookCommand { get; private set; } // 【新增】

        #region Constructors

        // 【新增】新的构造函数，用于离线模式（打开本地笔记）
        public ReadingViewModel(string title, string textContent, string audioFilePath, string folderPath, FlowDocument document = null)
        {
            _isOfflineMode = true; // 标记为离线模式
            _localNoteFolderPath = folderPath; // 保存本地笔记的文件夹路径
            // 1. 初始化所有通用命令
            InitializeCommonCommands();

            // 2. 【核心修正】将“上一篇/下一篇”命令设为永远不可用
            NextArticleCommand = new RelayCommand(_ => { }, _ => false);
            PreviousArticleCommand = new RelayCommand(_ => { }, _ => false);

            // 3. 设置离线模式的属性
            ArticleTitle = title;
            // 【核心修正】为纯文本内容添加标题
            ArticleContent = $"{title}\n\n{textContent}";
            PreloadedDocument = document; // PreloadedDocument 需要在类中定义

            // 4. 准备播放器
            // 为本地文件准备播放器
            PrepareAndOpenMediaPlayer(new Uri(audioFilePath, UriKind.Absolute));

            //_mediaPlayer.Open(new Uri(audioFilePath, UriKind.Absolute));
            StatusText = "本地笔记已加载";
        }
        public ReadingViewModel(List<ArticleViewModel> playlist, VoaScraperService scraperService)
        {
            // 1. 初始化特定于在线模式的字段
            _playlist = playlist;
            _scraperService = scraperService;
            _isOfflineMode = false; // 标记为在线模式

            // 2. 初始化所有通用命令
            InitializeCommonCommands();

            // 3. 初始化特定于在线模式的命令
            NextArticleCommand = new RelayCommand(async _ => await LoadArticleAsync(_currentIndex + 1), _ => !IsBusy && _playlist != null && _currentIndex < _playlist.Count - 1);
            PreviousArticleCommand = new RelayCommand(async _ => await LoadArticleAsync(_currentIndex - 1), _ => !IsBusy && _playlist != null && _currentIndex > 0);

            // 4. 启动加载
            _ = LoadArticleAsync(0);
        }
        #endregion

        /// <summary>
        /// 初始化所有模式共用的命令
        /// </summary>
        private void InitializeCommonCommands()
        {
            PlayPauseCommand = new RelayCommand(_ => TogglePlayPause(), _ => !IsBusy);
            WindowClosingCommand = new RelayCommand(_ => CleanUp());
            HighlightCommand = new RelayCommand(_ => _view?.ApplyHighlight(), _ => _view != null);
            UnderlineCommand = new RelayCommand(_ => _view?.ApplyUnderline(), _ => _view != null);
            EraserCommand = new RelayCommand(_ => _view?.ClearFormatting(), _ => _view != null);
            ExportNotesCommand = new RelayCommand(async _ => await ExportNotesAsync(), _ => !IsBusy);
            AddToVocabCommand = new RelayCommand(_ => AddCurrentWordToVocab(), _ => CanAddToVocab);
            ViewVocabBookCommand = new RelayCommand(_ => OpenVocabBookWindow());
            SendMessageCommand = new RelayCommand(async _ => await SendMessageAsync(), _ => !IsAiThinking && !string.IsNullOrWhiteSpace(UserInput));

            //_mediaPlayer.Open(new Uri(localAudioPath, UriKind.Absolute));

            // 初始化计时器等
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += OnTimerTick;

            // 【新增】初始化倍速命令
            AvailableSpeedRatios = new List<SpeedRatioOption>
            {
                new SpeedRatioOption { DisplayName = "0.75x", Value = 0.75 },
                new SpeedRatioOption { DisplayName = "1.0x", Value = 1.0 },
                new SpeedRatioOption { DisplayName = "1.25x", Value = 1.25 },
                new SpeedRatioOption { DisplayName = "1.5x", Value = 1.5 },
            };
            SetSpeedCommand = new RelayCommand(param => {
                if (param is double speed)
                {
                    _mediaPlayer.SpeedRatio = speed;
                    CurrentSpeedRatio = speed;
                }
            });

            // 【新增】初始化拖动命令
            SliderDragStartedCommand = new RelayCommand(_ => _isUserDraggingSlider = true);
            SliderDragCompletedCommand = new RelayCommand(param =>
            {
                if (param is double newPosition && _mediaPlayer.CanPause) // 确保媒体已加载
                {
                    _mediaPlayer.Position = TimeSpan.FromSeconds(newPosition);
                }
                _isUserDraggingSlider = false;
            });
            // 【新增】初始化新的保存命令
            SaveOrUpdateNotesCommand = new RelayCommand(async _ => await SaveOrUpdateNotesAsync(), _ => !IsBusy);
            // 【新增】初始化新命令
            AddToPhraseBookCommand = new RelayCommand(
                _ => AddSelectionToPhraseBook(),
                _ => !string.IsNullOrEmpty(_view?.GetSelectedText()) // 只有选中了文本才能收藏
            );
        }

        // 【新增】将选中内容添加到积累本的方法
        private void AddSelectionToPhraseBook()
        {
            string selectedText = _view?.GetSelectedText();
            if (!string.IsNullOrEmpty(selectedText))
            {
                PhraseDataService.AddPhrase(selectedText, ArticleTitle);
                StatusText = "已成功收藏！";
            }
        }

        // 【新增】统一的保存/更新逻辑
        private async Task SaveOrUpdateNotesAsync()
        {
            if (_isOfflineMode)
            {
                await UpdateLocalNotesAsync(); // 如果是离线模式，则更新
            }
            else
            {
                await SaveNewNotesAsync(); // 如果是在线模式，则保存为新笔记
            }
        }

        // 【新增】更新本地笔记的逻辑
        private async Task UpdateLocalNotesAsync()
        {
            IsBusy = true;
            StatusText = "正在保存更改...";
            try
            {
                string baseFileName = new DirectoryInfo(_localNoteFolderPath).Name;
                string mdFilePath = Path.Combine(_localNoteFolderPath, $"{baseFileName}.md");

                var document = _view?.GetDocument();
                if (document == null) throw new Exception("无法获取笔记内容。");

                string markdownContent = MarkdownExportService.ConvertToMarkdown(document);
                await File.WriteAllTextAsync(mdFilePath, markdownContent);

                StatusText = "笔记已成功保存！";
                MessageBox.Show("笔记更改已成功保存！", "保存完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                StatusText = "保存失败";
                MessageBox.Show($"保存笔记时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // 原来的“导出”逻辑，现在是“保存新笔记”
        private async Task SaveNewNotesAsync()
        {
            if (_currentArticle == null) return;

            var dialog = new OpenFolderDialog { Title = "请选择一个根目录来保存文章文件夹" };

            if (dialog.ShowDialog() == true)
            {
                IsBusy = true;
                StatusText = "正在导出笔记...";
                try
                {
                    // 用户选择的根目录
                    string rootFolderPath = dialog.FolderName;
                    string baseFileName = _scraperService.MakeValidFileName(_currentArticle.Title);

                    // =============================================================
                    // ====         【核心修正】创建并使用子文件夹              ====
                    // =============================================================

                    // 1. 根据文章标题创建一个新的子文件夹路径
                    string articleFolderPath = Path.Combine(rootFolderPath, baseFileName);

                    // 2. 确保这个子文件夹存在于磁盘上
                    Directory.CreateDirectory(articleFolderPath);

                    // =============================================================

                    // 3. 获取带格式的文档
                    var document = _view?.GetDocument();
                    if (document == null) throw new Exception("无法获取笔记内容。");

                    // 4. 转换并保存Markdown文件到【新子文件夹】中
                    string markdownContent = MarkdownExportService.ConvertToMarkdown(document);
                    await File.WriteAllTextAsync(Path.Combine(articleFolderPath, $"{baseFileName}.md"), markdownContent);

                    // 5. 保存原始文本文件到【新子文件夹】中
                    await File.WriteAllTextAsync(Path.Combine(articleFolderPath, $"{baseFileName}.txt"), ArticleContent);

                    // 6. 保存音频文件到【新子文件夹】中
                    if (!string.IsNullOrEmpty(_currentAudioUrl))
                    {
                        StatusText = "正在下载音频文件...";
                        var audioPath = Path.Combine(articleFolderPath, $"{baseFileName}.mp3");
                        await _scraperService.DownloadFileWithProgressAsync(
                            fileUrl: _currentAudioUrl,
                            destinationPath: audioPath,
                            referer: _currentArticle.Url,
                            progress: new Progress<double>()
                        );
                    }

                    StatusText = "笔记导出成功！";
                    // 在成功消息中，显示新的完整文件夹路径
                    MessageBox.Show($"笔记已成功导出到:\n{articleFolderPath}", "导出完成", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    StatusText = "导出失败";
                    MessageBox.Show($"导出笔记时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        // 【全新】提供一个方法让View将自身注入进来
        public void SetView(IReadingView view)
        {
            _view = view;
        }

        // 【新增】打开生词本窗口的方法
        private void OpenVocabBookWindow()
        {
            var vocabViewModel = new VocabBookViewModel();
            var vocabWindow = new VocabBookWindow
            {
                DataContext = vocabViewModel
            };
            vocabWindow.Show(); // 使用Show()，这样可以同时操作两个窗口
        }

        // 【修改】查词方法，现在会同时更新两个属性
        public void LookupWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                SelectedWord = "";
                SelectedWordDefinition = "在左侧选择单词以查看释义。";
                return;
            }

            SelectedWord = word; // 保存查询的原单词
            SelectedWordDefinition = DictionaryService.GetDefinition(word);
            // 【新增】更新按钮可用状态
            // 只有当单词不为空，且不是“未收录”提示时，按钮才可用
            CanAddToVocab = !string.IsNullOrWhiteSpace(SelectedWord) &&
                            !SelectedWordDefinition.Contains("未收录");
        }

        private void AddCurrentWordToVocab()
        {
            if (CanAddToVocab)
            {
                VocabDataService.AddOrUpdateWord(SelectedWord, SelectedWordDefinition);
                // 提供反馈
                StatusText = $"单词 '{SelectedWord}' 已加入生词本！";
            }
        }

        private async Task LoadArticleAsync(int index)
        {
            IsPlaying = false;
            CurrentPosition = 0;
            MaximumPosition = 1;
            CleanUp();

            IsBusy = true;
            StatusText = $"正在加载 ({index + 1}/{_playlist.Count})...";

            _currentIndex = index;
            _currentArticle = _playlist[_currentIndex];
            ArticleTitle = _currentArticle.Title;

            try
            {
                var (content, audioUrl) = await _scraperService.GetArticleDetailsAsync(_currentArticle.Url);
                _currentAudioUrl = audioUrl;
                // 【核心修正】为在线获取的纯文本内容添加标题
                string rawContent = content ?? "内容加载失败。";
                ArticleContent = $"{ArticleTitle}\n\n{rawContent}";

                if (!string.IsNullOrEmpty(_currentAudioUrl))
                {
                    StatusText = "正在缓存音频...";
                    string validFileName = _scraperService.MakeValidFileName(_currentArticle.Title) + ".mp3";
                    string localAudioPath = await _scraperService.DownloadAudioToCacheAsync(_currentAudioUrl, validFileName, _currentArticle.Url);

                    if (string.IsNullOrEmpty(localAudioPath))
                    {
                        throw new Exception("音频文件缓存失败，请检查网络或稍后再试。");
                    }

                    PrepareAndOpenMediaPlayer(new Uri(localAudioPath, UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                StatusText = "加载失败";
                MessageBox.Show($"加载文章时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                IsBusy = false; // 确保在出错时也重置繁忙状态
            }
            // IsBusy 的重置现在由 OnMediaOpened 或 catch 块处理
        }

        /// <summary>
        /// 【核心修正】准备一个新的MediaPlayer实例并打开媒体
        /// </summary>
        private void PrepareAndOpenMediaPlayer(Uri mediaUri)
        {
            CleanUp();
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += OnMediaOpened;
            _mediaPlayer.MediaEnded += OnMediaEnded;
            _mediaPlayer.MediaFailed += OnMediaFailed;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            _timer.Tick += OnTimerTick;
            _mediaPlayer.Open(mediaUri);
        }

        // =============================================================
        // ====         【核心修正】所有播放逻辑移至此处           ====
        // =============================================================
        // ReadingViewModel.cs

        private void OnMediaOpened(object sender, EventArgs e)
        {
            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                MaximumPosition = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

                // 【修改】根据模式设置状态文本
                if (_playlist != null)
                {
                    StatusText = $"正在播放 ({_currentIndex + 1}/{_playlist.Count})";
                }
                else
                {
                    StatusText = "正在播放";
                }

                // 【新增】将“正在播放”的状态文本保存起来
                _lastPlayingStatusText = StatusText;

                // 自动开始播放
                _mediaPlayer.Play();
                _timer.Start();
                IsPlaying = true;
            }
            IsBusy = false;
        }
        private void OnMediaEnded(object sender, EventArgs e)
        {
            // 播放结束后重置状态
            IsPlaying = false;
            _timer.Stop();
            CurrentPosition = 0;
            _mediaPlayer.Stop(); // 确保播放头回到起点
        }
        private void OnMediaFailed(object sender, ExceptionEventArgs e)
        {
            StatusText = "音频播放失败！";
            MessageBox.Show($"无法播放音频文件。\n错误: {e.ErrorException.Message}", "播放错误", MessageBoxButton.OK, MessageBoxImage.Error);
            IsBusy = false; // 媒体失败后，取消繁忙状态
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            // 【核心修正】当用户没有在拖动滑块时，才更新进度条位置
            if (!_isUserDraggingSlider)
            {
                CurrentPosition = _mediaPlayer.Position.TotalSeconds;
            }
        }

        // ReadingViewModel.cs

        private void TogglePlayPause()
        {
            if (IsBusy) return;
            IsPlaying = !IsPlaying;

            if (IsPlaying)
            {
                // 从暂停 -> 播放
                _mediaPlayer.Play();
                _timer.Start();
                // 【核心修正】恢复之前保存的“正在播放”文本
                StatusText = _lastPlayingStatusText;
            }
            else
            {
                // 从播放 -> 暂停
                _mediaPlayer.Pause();
                _timer.Stop();
                // 【核心修正】将状态文本设置为“已暂停”
                StatusText = "已暂停";
            }
        }

        private void CleanUp()
        {
            if (_mediaPlayer != null)
            {
                // 停止并注销所有事件，为GC回收做准备
                _mediaPlayer.Stop();
                _mediaPlayer.MediaOpened -= OnMediaOpened;
                _mediaPlayer.MediaEnded -= OnMediaEnded;
                _mediaPlayer.MediaFailed -= OnMediaFailed;
                _mediaPlayer.Close();
                _mediaPlayer = null;
            }
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= OnTimerTick;
                _timer = null;
            }
        }

        // 【已重构】发送消息给AI的方法
        private async Task SendMessageAsync()
        {
            if (IsAiThinking || string.IsNullOrWhiteSpace(UserInput)) return;

            IsAiThinking = true;
            string userMessage = UserInput;
            UserInput = ""; // 清空输入框后，手动触发一次属性变更通知
            OnPropertyChanged(nameof(UserInput));

            // 1. 将用户消息添加到历史记录
            ChatHistory.Add(new ChatMessage { Author = "You", Content = userMessage, IsUserMessage = true });

            try
            {
                // 2. 检查并初始化AI服务 (惰性加载)
                if (_aiService == null)
                {
                    var apiKey = App.Configuration?["Gemini:ApiKey"];
                    if (string.IsNullOrEmpty(apiKey))
                    {
                        ChatHistory.Add(new ChatMessage { Author = "AI", Content = "错误：未在appsettings.json中配置Gemini API密钥。", IsUserMessage = false });
                        return; // 提前退出
                    }
                    _aiService = new GeminiAiService(apiKey);

                    // 使用当前选中的文本作为上下文，开启聊天会话
                    string context = _view?.GetSelectedText() ?? "No specific context selected.";
                    _aiService.StartChatSession(context);
                }

                // 3. 发送消息并获取回复
                string aiResponse = await _aiService.SendMessageAsync(userMessage);

                // 4. 将AI的回复添加到历史记录
                ChatHistory.Add(new ChatMessage { Author = "AI", Content = aiResponse.Trim(), IsUserMessage = false });
            }
            catch (Exception ex)
            {
                ChatHistory.Add(new ChatMessage { Author = "AI", Content = $"抱歉，处理时出现错误: {ex.Message}", IsUserMessage = false });
            }
            finally
            {
                IsAiThinking = false;
            }
        }

        // 【新增】导出笔记的完整方法
        // 【已修正】导出笔记的完整方法
        private async Task ExportNotesAsync()
        {
            if (_currentArticle == null) return;

            var dialog = new OpenFolderDialog
            {
                Title = "请选择一个根目录来保存文章文件夹"
            };

            if (dialog.ShowDialog() == true)
            {
                IsBusy = true;
                StatusText = "正在导出笔记...";
                try
                {
                    // 用户选择的根目录
                    string rootFolderPath = dialog.FolderName;
                    string baseFileName = _scraperService.MakeValidFileName(_currentArticle.Title);

                    // =============================================================
                    // ====         【核心修正】创建并使用子文件夹              ====
                    // =============================================================

                    // 1. 根据文章标题创建一个新的子文件夹路径
                    string articleFolderPath = Path.Combine(rootFolderPath, baseFileName);

                    // 2. 确保这个子文件夹存在于磁盘上
                    Directory.CreateDirectory(articleFolderPath);

                    // =============================================================

                    // 3. 获取带格式的文档
                    var document = _view?.GetDocument();
                    if (document == null) throw new Exception("无法获取笔记内容。");

                    // 4. 转换并保存Markdown文件到【新子文件夹】中
                    string markdownContent = MarkdownExportService.ConvertToMarkdown(document);
                    await File.WriteAllTextAsync(Path.Combine(articleFolderPath, $"{baseFileName}.md"), markdownContent);

                    // 5. 保存原始文本文件到【新子文件夹】中
                    await File.WriteAllTextAsync(Path.Combine(articleFolderPath, $"{baseFileName}.txt"), ArticleContent);

                    // 6. 保存音频文件到【新子文件夹】中
                    if (!string.IsNullOrEmpty(_currentAudioUrl))
                    {
                        StatusText = "正在下载音频文件...";
                        var audioPath = Path.Combine(articleFolderPath, $"{baseFileName}.mp3");
                        await _scraperService.DownloadFileWithProgressAsync(
                            fileUrl: _currentAudioUrl,
                            destinationPath: audioPath,
                            referer: _currentArticle.Url,
                            progress: new Progress<double>()
                        );
                    }

                    StatusText = "笔记导出成功！";
                    // 在成功消息中，显示新的完整文件夹路径
                    MessageBox.Show($"笔记已成功导出到:\n{articleFolderPath}", "导出完成", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    StatusText = "导出失败";
                    MessageBox.Show($"导出笔记时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
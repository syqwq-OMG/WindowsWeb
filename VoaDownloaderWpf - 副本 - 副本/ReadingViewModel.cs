using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace VoaDownloaderWpf
{
    public class ReadingViewModel : BaseViewModel
    {
        // --- 依赖与状态 ---
        private readonly VoaScraperService _scraperService;
        private readonly List<ArticleViewModel> _playlist;
        private int _currentIndex = -1;
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isUserDraggingSlider = false;
        private IReadingView _view; // 新增：持有对视图接口的引用

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

        // 【新增】AI聊天相关属性
        public ObservableCollection<ChatMessage> ChatHistory { get; }
        private string _userInput;
        public string UserInput { get => _userInput; set { _userInput = value; OnPropertyChanged(); } }
        private bool _isAiThinking;
        public bool IsAiThinking { get => _isAiThinking; set { _isAiThinking = value; OnPropertyChanged(); } }


        // --- 命令 ---
        public ICommand PlayPauseCommand { get; }
        public ICommand NextArticleCommand { get; }
        public ICommand PreviousArticleCommand { get; }
        public ICommand WindowClosingCommand { get; }
        public ICommand SliderDragStartedCommand { get; }
        public ICommand SliderDragCompletedCommand { get; }
        // 【全新】定义与UI绑定的命令
        public ICommand HighlightCommand { get; }
        public ICommand UnderlineCommand { get; }
        public ICommand EraserCommand { get; }
        // 【新增】查看生词本命令
        public ICommand ViewVocabBookCommand { get; }
        // 【新增】加入生词本命令
        public ICommand AddToVocabCommand { get; }

        // 【新增】AI聊天相关命令
        public ICommand SendMessageCommand { get; }

        public ReadingViewModel(List<ArticleViewModel> playlist, VoaScraperService scraperService)
        {
            _playlist = playlist;
            _scraperService = scraperService;

            // 初始化命令
            PlayPauseCommand = new RelayCommand(_ => TogglePlayPause(), _ => !IsBusy);
            NextArticleCommand = new RelayCommand(async _ => await LoadArticleAsync(_currentIndex + 1), _ => !IsBusy && _currentIndex < _playlist.Count - 1);
            PreviousArticleCommand = new RelayCommand(async _ => await LoadArticleAsync(_currentIndex - 1), _ => !IsBusy && _currentIndex > 0);
            WindowClosingCommand = new RelayCommand(_ => CleanUp());
            SliderDragStartedCommand = new RelayCommand(_ => _isUserDraggingSlider = true);
            SliderDragCompletedCommand = new RelayCommand(param => {
                if (param is double value && !IsBusy) { _mediaPlayer.Position = TimeSpan.FromSeconds(value); _isUserDraggingSlider = false; }
            });
            // 【全新】初始化标记相关的命令
            HighlightCommand = new RelayCommand(_ => _view?.ApplyHighlight(), _ => _view != null);
            UnderlineCommand = new RelayCommand(_ => _view?.ApplyUnderline(), _ => _view != null);
            EraserCommand = new RelayCommand(_ => _view?.ClearFormatting(), _ => _view != null);
            ViewVocabBookCommand = new RelayCommand(_ => OpenVocabBookWindow());

            // 【新增】初始化加入生词本命令
            AddToVocabCommand = new RelayCommand(
                execute: _ => AddCurrentWordToVocab(),
                canExecute: _ => CanAddToVocab
            );

            // 【新增】初始化聊天相关内容
            ChatHistory = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new RelayCommand(async _ => await SendMessageAsync(), _ => !IsAiThinking && !string.IsNullOrWhiteSpace(UserInput));


            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += OnTimerTick;

            // 启动时加载第一篇文章
            _ = LoadArticleAsync(0);
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
            // 1. 设置加载状态
            IsBusy = true;
            CleanUp(); // 清理上一篇文章的播放器资源
            StatusText = $"正在加载 ({index + 1}/{_playlist.Count})...";

            // 2. 更新当前索引和UI
            _currentIndex = index;
            var currentArticle = _playlist[_currentIndex];
            ArticleTitle = currentArticle.Title;
            ArticleContent = "正在获取内容..."; // 临时内容

            try
            {
                // 3. 获取文章数据并缓存音频
                var (content, audioUrl) = await _scraperService.GetArticleDetailsAsync(currentArticle.Url);
                ArticleContent = content ?? "内容加载失败。";

                if (string.IsNullOrEmpty(audioUrl))
                {
                    StatusText = "未找到音频文件";
                    IsBusy = false;
                    return;
                }

                StatusText = "正在缓存音频...";
                string validFileName = _scraperService.MakeValidFileName(currentArticle.Title) + ".mp3";
                string localAudioPath = await _scraperService.DownloadAudioToCacheAsync(audioUrl, validFileName, currentArticle.Url);

                if (string.IsNullOrEmpty(localAudioPath))
                {
                    StatusText = "音频缓存失败";
                    IsBusy = false;
                    return;
                }

                // 4. 准备新的播放器实例
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.MediaOpened += OnMediaOpened;
                _mediaPlayer.MediaFailed += OnMediaFailed;
                _mediaPlayer.Open(new Uri(localAudioPath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                StatusText = "加载时出错";
                ArticleContent = $"加载文章时出错: {ex.Message}";
                IsBusy = false;
            }
        }

        private void OnMediaOpened(object sender, EventArgs e)
        {
            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                MaximumPosition = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                StatusText = $"就绪 ({_currentIndex + 1}/{_playlist.Count})";
                IsBusy = false; // 加载完成
                TogglePlayPause(); // 自动播放
            }
        }

        private void OnMediaFailed(object sender, ExceptionEventArgs e)
        {
            StatusText = "音频播放失败！";
            IsBusy = false;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (!_isUserDraggingSlider) { CurrentPosition = _mediaPlayer.Position.TotalSeconds; }
        }

        private void TogglePlayPause()
        {
            if (IsBusy) return;
            IsPlaying = !IsPlaying;
            if (IsPlaying) { _mediaPlayer.Play(); _timer.Start(); }
            else { _mediaPlayer.Pause(); _timer.Stop(); }
        }

        private void CleanUp()
        {
            if (_mediaPlayer != null)
            {
                IsPlaying = false;
                _timer.Stop();
                _mediaPlayer.MediaOpened -= OnMediaOpened;
                _mediaPlayer.MediaFailed -= OnMediaFailed;
                _mediaPlayer.Stop();
                _mediaPlayer.Close();
            }
            // 重置UI状态
            CurrentPosition = 0;
            MaximumPosition = 1;
        }

        // 【已重构】发送消息给AI的方法
        private async Task SendMessageAsync()
        {
            if (IsAiThinking || string.IsNullOrWhiteSpace(UserInput)) return;

            IsAiThinking = true;
            string userMessage = UserInput;
            UserInput = ""; // 清空输入框

            // 1. 将用户消息添加到UI历史记录
            ChatHistory.Add(new ChatMessage { Author = "You", Content = userMessage, IsUserMessage = true });

            try
            {
                // 2. 检查AI服务是否已初始化
                if (_aiService == null)
                {
                    var apiKey = App.Configuration["Gemini:ApiKey"];
                    if (string.IsNullOrEmpty(apiKey))
                    {
                        ChatHistory.Add(new ChatMessage { Author = "AI", Content = "错误：未在appsettings.json中配置Gemini API密钥。", IsUserMessage = false });
                        IsAiThinking = false;
                        return;
                    }
                    _aiService = new GeminiAiService(apiKey);

                    // 使用当前选中的文本作为上下文，开启聊天会话
                    string context = _view?.GetSelectedText() ?? "No specific context selected.";
                    _aiService.StartChatSession(context);
                }

                // 3. 发送消息并获取回复
                string aiResponse = await _aiService.SendMessageAsync(userMessage);

                // 4. 将AI的回复添加到UI历史记录
                ChatHistory.Add(new ChatMessage { Author = "AI", Content = aiResponse.Trim(), IsUserMessage = false });
            }
            finally
            {
                IsAiThinking = false;
            }
        }
    }
}
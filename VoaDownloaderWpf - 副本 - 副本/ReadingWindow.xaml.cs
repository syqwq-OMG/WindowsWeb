using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace VoaDownloaderWpf
{
    // 在这里实现IReadingView接口
    public partial class ReadingWindow : Window, IReadingView
    {
        private ReadingViewModel _viewModel;

        public ReadingWindow()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        // 当ViewModel设置好后，将View自身(作为IReadingView)传递给ViewModel
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ReadingViewModel viewModel)
            {
                _viewModel = viewModel;
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                _viewModel.SetView(this); // 建立ViewModel到View的通信桥梁
                LoadArticleContentIntoRichTextBox();
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReadingViewModel.ArticleContent))
            {
                Dispatcher.Invoke(LoadArticleContentIntoRichTextBox);
            }
        }

        private void ArticleRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string selectedText = ArticleRichTextBox.Selection.Text.Trim();

            // 简单判断是否为一个单词 (不包含空格等)
            if (!string.IsNullOrEmpty(selectedText) && !selectedText.Contains(" "))
            {
                _viewModel?.LookupWord(selectedText);
            }
        }

        private void LoadArticleContentIntoRichTextBox()
        {
            if (_viewModel?.ArticleContent != null)
            {
                ArticleRichTextBox.IsEnabled = false;
                ArticleRichTextBox.Document.Blocks.Clear();
                ArticleRichTextBox.Document.Blocks.Add(new Paragraph(new Run(_viewModel.ArticleContent)));
                ArticleRichTextBox.IsEnabled = true;
            }
        }

        // =============================================================
        // === 实现 IReadingView 接口中定义的所有方法 (核心逻辑) ===
        // =============================================================

        public void ApplyHighlight()
        {
            ApplyFormatting(TextElement.BackgroundProperty, Brushes.Yellow);
        }

        public void ApplyUnderline()
        {
            var selection = ArticleRichTextBox.Selection;
            if (selection.IsEmpty) return;

            var currentDecorations = selection.GetPropertyValue(Inline.TextDecorationsProperty);
            if (currentDecorations != DependencyProperty.UnsetValue && currentDecorations.Equals(TextDecorations.Underline))
            {
                ApplyFormatting(Inline.TextDecorationsProperty, null);
            }
            else
            {
                ApplyFormatting(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
        }

        public void ClearFormatting()
        {
            ApplyFormatting(TextElement.BackgroundProperty, null);
            ApplyFormatting(Inline.TextDecorationsProperty, null);
        }

        private void ApplyFormatting(DependencyProperty property, object value)
        {
            TextSelection selection = ArticleRichTextBox.Selection;
            if (selection.IsEmpty) return;

            ArticleRichTextBox.IsReadOnly = false;
            selection.ApplyPropertyValue(property, value);
            ArticleRichTextBox.IsReadOnly = true;
            ArticleRichTextBox.Focus();
        }

        public string GetSelectedText()
        {
            return ArticleRichTextBox.Selection.Text;
        }
    }
}
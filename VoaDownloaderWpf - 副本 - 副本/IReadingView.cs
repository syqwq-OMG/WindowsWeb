// IReadingView.cs (或者放在 ReadingWindow.xaml.cs 文件的顶部)

using System.Windows.Documents;

namespace VoaDownloaderWpf
{
    public interface IReadingView
    {
        void ApplyHighlight();
        void ApplyUnderline();
        void ClearFormatting();
        // ... (保留之前的方法)
        string GetSelectedText(); // 【新增】
        FlowDocument GetDocument(); // 【新增】获取完整的FlowDocument
    }
}
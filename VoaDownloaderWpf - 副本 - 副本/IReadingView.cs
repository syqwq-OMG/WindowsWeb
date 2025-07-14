// IReadingView.cs (或者放在 ReadingWindow.xaml.cs 文件的顶部)

namespace VoaDownloaderWpf
{
    public interface IReadingView
    {
        void ApplyHighlight();
        void ApplyUnderline();
        void ClearFormatting();
        // ... (保留之前的方法)
        string GetSelectedText(); // 【新增】
    }
}
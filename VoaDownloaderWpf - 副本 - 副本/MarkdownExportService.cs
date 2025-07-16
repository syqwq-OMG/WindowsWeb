// MarkdownExportService.cs
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace VoaDownloaderWpf
{
    public static class MarkdownExportService
    {
        public static string ConvertToMarkdown(FlowDocument document)
        {
            var markdownBuilder = new StringBuilder();

            foreach (var block in document.Blocks)
            {
                if (block is Paragraph paragraph)
                {
                    foreach (var inline in paragraph.Inlines)
                    {
                        if (inline is Run run)
                        {
                            string text = run.Text;

                            // 检查格式
                            bool isBold = run.Background is SolidColorBrush brush && brush.Color == Colors.Yellow;
                            bool isUnderlined = run.TextDecorations.Contains(TextDecorations.Underline[0]);

                            // 根据格式应用Markdown语法
                            if (isBold && isUnderlined)
                            {
                                markdownBuilder.Append($"<u>**{text}**</u>");
                            }
                            else if (isBold)
                            {
                                markdownBuilder.Append($"**{text}**");
                            }
                            else if (isUnderlined)
                            {
                                markdownBuilder.Append($"<u>{text}</u>");
                            }
                            else
                            {
                                markdownBuilder.Append(text);
                            }
                        }
                    }
                    // 每个段落后添加两个换行符
                    markdownBuilder.AppendLine("\n");
                }
            }
            return markdownBuilder.ToString();
        }
    }
}
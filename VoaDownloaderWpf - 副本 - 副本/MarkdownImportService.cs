// MarkdownImportService.cs
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace VoaDownloaderWpf
{
    public static class MarkdownImportService
    {
        public static FlowDocument ConvertToFlowDocument(string markdownText, string title = null)
        {
            var flowDocument = new FlowDocument();
            // 1. 【新增】如果传入了标题，就创建一个居中的、加粗的大标题
            if (!string.IsNullOrEmpty(title))
            {
                Paragraph titleParagraph = new Paragraph(new Run(title))
                {
                    FontSize = 22,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20) // 标题和正文间距
                };
                flowDocument.Blocks.Add(titleParagraph);
            }

            var paragraph = new Paragraph();

            // 正则表达式模式保持不变，但我们会改变使用它的方式
            const string pattern = @"(\*\*.*?\*\*|<u>.*?</u>)";

            // 使用 RegexOptions.Singleline 确保 “.” 可以匹配换行符，从而支持跨行格式
            var regex = new Regex(pattern, RegexOptions.Singleline);
            var matches = regex.Matches(markdownText);

            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // 1. 添加上一个匹配项和当前匹配项之间的普通文本
                if (match.Index > lastIndex)
                {
                    string plainText = markdownText.Substring(lastIndex, match.Index - lastIndex);
                    paragraph.Inlines.Add(new Run(plainText));
                }

                // 2. 处理当前匹配到的带格式的文本
                string formattedPart = match.Value;
                Run formattedRun = new Run();

                if (formattedPart.StartsWith("**") && formattedPart.EndsWith("**"))
                {
                    // 提取 ** 和 ** 中间的文本
                    formattedRun.Text = formattedPart.Substring(2, formattedPart.Length - 4);
                    // 荧光笔高亮
                    formattedRun.Background = Brushes.Yellow;
                }
                else if (formattedPart.StartsWith("<u>") && formattedPart.EndsWith("</u>"))
                {
                    // 提取 <u> 和 </u> 中间的文本
                    formattedRun.Text = formattedPart.Substring(3, formattedPart.Length - 7);
                    // 下划线
                    formattedRun.TextDecorations.Add(TextDecorations.Underline);
                }

                paragraph.Inlines.Add(formattedRun);

                // 3. 更新索引，为下一次循环做准备
                lastIndex = match.Index + match.Length;
            }

            // 4. 添加最后一个匹配项之后的所有剩余普通文本
            if (lastIndex < markdownText.Length)
            {
                string remainingText = markdownText.Substring(lastIndex);
                paragraph.Inlines.Add(new Run(remainingText));
            }

            flowDocument.Blocks.Add(paragraph);
            return flowDocument;
        }
    }
}
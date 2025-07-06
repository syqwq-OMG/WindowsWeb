using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoaDownloader
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string BaseUrl = "https://www.21voa.com/";
        private readonly Dictionary<string, string> categoryLinks = new Dictionary<string, string>();
        private readonly Dictionary<string, string> articleLinks = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            SetControlsEnabled(false);
            label_Status.Text = "正在加载分类...";
            try
            {
                await LoadCategoriesAsync();
                if (comboBox_Category.Items.Count > 0)
                {
                    comboBox_Category.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载分类列表失败: {ex.Message}", "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                label_Status.Text = "就绪";
                SetControlsEnabled(true);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var web = new HtmlWeb();

            var doc = await web.LoadFromWebAsync(BaseUrl);
            var categoryNodes = doc.DocumentNode.SelectNodes("//div[@id='lefter']/ul/li/a");

            if (categoryNodes != null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    comboBox_Category.Items.Clear();
                    categoryLinks.Clear();
                    foreach (var node in categoryNodes)
                    {
                        string name = WebUtility.HtmlDecode(node.InnerText.Trim());
                        string link = node.GetAttributeValue("href", "");
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(link) && link.Contains(".html"))
                        {
                            string fullLink = new Uri(new Uri(BaseUrl), link).ToString();
                            if (!categoryLinks.ContainsKey(name))
                            {
                                comboBox_Category.Items.Add(name);
                                categoryLinks[name] = fullLink;
                            }
                        }
                    }
                });
            }
        }

        private async void button_Fetch_Click(object sender, EventArgs e)
        {
            if (comboBox_Category.SelectedItem == null) return;
            string selectedCategory = comboBox_Category.SelectedItem.ToString();
            if (!categoryLinks.ContainsKey(selectedCategory)) return;

            SetControlsEnabled(false);
            label_Status.Text = "正在获取文章列表...";
            progressBar.Value = 0;
            checkedListBox_Articles.Items.Clear();
            articleLinks.Clear();
            richTextBox_Content.Clear();
            checkBox_SelectAll.Checked = false;

            try
            {
                string categoryUrl = categoryLinks[selectedCategory];
                int page = (int)numericUpDown_Page.Value;
                string pageUrl = page > 1 ? $"{categoryUrl.Replace(".html", "")}_{page}.html" : categoryUrl;

                var web = new HtmlWeb();

                var doc = await web.LoadFromWebAsync(pageUrl);
                var articleNodes = doc.DocumentNode.SelectNodes("//div[@class='list']/ul/li/a");
                if (articleNodes != null)
                {
                    foreach (var node in articleNodes)
                    {
                        string title = WebUtility.HtmlDecode(node.InnerText.Trim());
                        string link = node.GetAttributeValue("href", "");
                        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(link) && link.Contains(".html"))
                        {
                            string fullLink = new Uri(new Uri(BaseUrl), link).ToString();
                            if (!articleLinks.ContainsKey(title))
                            {
                                articleLinks[title] = fullLink;
                                checkedListBox_Articles.Items.Add(title);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取文章列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                label_Status.Text = "就绪";
                SetControlsEnabled(true);
            }
        }

        private async void checkedListBox_Articles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox_Articles.SelectedItem == null) return;
            string title = checkedListBox_Articles.SelectedItem.ToString();
            if (!articleLinks.ContainsKey(title)) return;

            richTextBox_Content.Text = "正在加载预览...";
            try
            {
                var (content, _) = await GetArticleDetailsAsync(articleLinks[title]);
                richTextBox_Content.Text = content ?? "无法加载预览内容。";
            }
            catch (Exception ex)
            {
                richTextBox_Content.Text = $"加载预览失败: {ex.Message}";
            }
        }

        private void checkBox_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Articles.Items.Count; i++)
            {
                checkedListBox_Articles.SetItemChecked(i, checkBox_SelectAll.Checked);
            }
        }

        private async void button_Download_Click(object sender, EventArgs e)
        {
            var itemsToDownload = checkedListBox_Articles.CheckedItems.Cast<string>().ToList();
            if (!itemsToDownload.Any())
            {
                MessageBox.Show("请至少选择一篇文章进行下载。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "请选择一个用于保存所有下载内容的根文件夹";
                if (fbd.ShowDialog() != DialogResult.OK) return;

                SetControlsEnabled(false);
                var baseSavePath = fbd.SelectedPath;
                var failedDownloads = new List<string>();
                var progress = new Progress<int>(value => progressBar.Value = value);

                for (int i = 0; i < itemsToDownload.Count; i++)
                {
                    string title = itemsToDownload[i];
                    label_Status.Text = $"({i + 1}/{itemsToDownload.Count}) 正在下载: {title}";
                    progressBar.Value = 0; // 每个新文件下载前重置进度条

                    if (!await DownloadSingleArticleAsync(title, articleLinks[title], baseSavePath, progress))
                    {
                        failedDownloads.Add(title);
                    }
                }

                // 显示最终报告
                var report = new StringBuilder();
                report.AppendLine($"批量下载完成！共 {itemsToDownload.Count - failedDownloads.Count} 篇成功。");
                if (failedDownloads.Any())
                {
                    report.AppendLine("\n以下文章下载失败:");
                    foreach (var failed in failedDownloads) report.AppendLine($"- {failed}");
                }
                MessageBox.Show(report.ToString(), "下载报告", MessageBoxButtons.OK, MessageBoxIcon.Information);

                label_Status.Text = "就绪";
                progressBar.Value = 0;
                SetControlsEnabled(true);
            }
        }

        // 核心下载逻辑，增加了IProgress<T>参数用于报告进度
        private async Task<bool> DownloadSingleArticleAsync(string title, string articleUrl, string baseSavePath, IProgress<int> progress)
        {
            try
            {
                var (content, audioUrl) = await GetArticleDetailsAsync(articleUrl);
                if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(audioUrl)) return false;

                string sanitizedTitle = MakeValidFileName(title);
                string articleFolderPath = Path.Combine(baseSavePath, sanitizedTitle);
                Directory.CreateDirectory(articleFolderPath);

                string textPath = Path.Combine(articleFolderPath, $"{sanitizedTitle}.txt");
                string audioPath = Path.Combine(articleFolderPath, $"{sanitizedTitle}.mp3");

                await File.WriteAllTextAsync(textPath, content, Encoding.UTF8);

                var request = new HttpRequestMessage(HttpMethod.Get, audioUrl);
                request.Headers.Add("Referer", articleUrl);

                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                // 调用带进度报告的流复制方法
                await CopyToWithProgressAsync(response, audioPath, progress);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // 带有进度报告的流复制方法
        private async Task CopyToWithProgressAsync(HttpResponseMessage response, string filePath, IProgress<int> progress)
        {
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                long? totalBytes = response.Content.Headers.ContentLength;
                if (!totalBytes.HasValue)
                {
                    await stream.CopyToAsync(fileStream); // 如果没有总长度信息，则直接复制
                    progress.Report(100);
                    return;
                }

                long totalBytesRead = 0;
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;
                    int percentage = (int)((double)totalBytesRead / totalBytes.Value * 100);
                    progress.Report(percentage);
                }
            }
        }

        private async Task<(string content, string audioUrl)> GetArticleDetailsAsync(string articleUrl)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(articleUrl);
            var contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='content']");
            string content = contentNode != null ? WebUtility.HtmlDecode(contentNode.InnerText.Trim()) : null;
            string audioUrl = null;
            var scriptNodes = doc.DocumentNode.SelectNodes("//script");
            if (scriptNodes != null)
            {
                var mp3Regex = new Regex(@"mp3\s*:\s*""(https?://[^""]+\.mp3)""");
                foreach (var script in scriptNodes)
                {
                    var match = mp3Regex.Match(script.InnerText);
                    if (match.Success)
                    {
                        audioUrl = match.Groups[1].Value;
                        break;
                    }
                }
            }
            return (content, audioUrl);
        }

        private void SetControlsEnabled(bool enabled)
        {
            comboBox_Category.Enabled = enabled;
            numericUpDown_Page.Enabled = enabled;
            button_Fetch.Enabled = enabled;
            splitContainer1.Enabled = enabled;
            button_Download.Enabled = enabled;
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            return Regex.Replace(name, $"[{invalidChars}]+", "_");
        }
    }
}
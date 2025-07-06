using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http; // Required for HttpClient
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VoaDownloader
{
    public partial class Form1 : Form
    {
        // A single, static HttpClient instance is recommended for performance.
        private static readonly HttpClient httpClient = new HttpClient();

        private const string BaseUrl = "https://www.21voa.com/";
        private Dictionary<string, string> categoryLinks = new Dictionary<string, string>();
        private Dictionary<string, string> articleLinks = new Dictionary<string, string>();
        private string currentAudioUrl = "";
        private string currentArticleUrl = ""; // Store the article URL to use as the Referer

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            comboBox_Category.Enabled = false;
            comboBox_Category.Text = "���ڼ��ط���...";
            try
            {
                await LoadCategoriesAsync();

                if (comboBox_Category.Items.Count > 0)
                {
                    comboBox_Category.Enabled = true;
                    comboBox_Category.SelectedIndex = 0;
                    comboBox_Category.Text = comboBox_Category.Items[0].ToString();
                }
                else
                {
                    comboBox_Category.Text = "����ʧ��";
                    MessageBox.Show("δ�ܴ���վ�����κη��࣬����������Ժ����ԡ�", "���ش���", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���ط����б�ʧ�ܣ������������Ӳ���������\n������Ϣ: {ex.Message}", "���ش���", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox_Category.Text = "����ʧ��";
            }
        }

        private async System.Threading.Tasks.Task LoadCategoriesAsync()
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(BaseUrl);
            var categoryNodes = doc.DocumentNode.SelectNodes("//div[@id='lefter']/ul/li/a");

            if (categoryNodes != null)
            {
                this.Invoke((MethodInvoker)delegate {
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
            if (comboBox_Category.SelectedItem == null)
            {
                MessageBox.Show("��ѡ��һ�����ࡣ");
                return;
            }
            string selectedCategory = comboBox_Category.SelectedItem.ToString();
            if (!categoryLinks.ContainsKey(selectedCategory)) return;

            string categoryUrl = categoryLinks[selectedCategory];
            int page = int.TryParse(textBox_Page.Text.Trim(), out int p) && p > 0 ? p : 1;
            string pageUrl = page > 1 ? $"{categoryUrl.Replace(".html", "")}_{page}.html" : categoryUrl;

            this.Text = "���ڻ�ȡ�����б�...";
            button_Fetch.Enabled = false;

            try
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(pageUrl);
                listBox_Articles.Items.Clear();
                articleLinks.Clear();
                richTextBox_Content.Clear();
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
                                listBox_Articles.Items.Add(title);
                                articleLinks[title] = fullLink;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("�ڴ�ҳ��δ�ҵ������б�");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡ�����б�ʧ�ܣ�{ex.Message}");
            }
            finally
            {
                this.Text = "VOA Ӣ������������";
                button_Fetch.Enabled = true;
            }
        }

        private async void listBox_Articles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_Articles.SelectedItem == null) return;
            string selectedTitle = listBox_Articles.SelectedItem.ToString();
            if (!articleLinks.ContainsKey(selectedTitle)) return;

            currentArticleUrl = articleLinks[selectedTitle]; // Save the article URL for the Referer header

            richTextBox_Content.Text = "���ڼ����������ݣ����Ժ�...";
            button_Download.Enabled = false;
            button_Download.Text = "��������";

            try
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(currentArticleUrl);

                var contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='content']");
                richTextBox_Content.Text = contentNode != null ? WebUtility.HtmlDecode(contentNode.InnerText.Trim()) : "δ�ܳɹ���ȡ���������ݡ�";

                currentAudioUrl = "";
                var scriptNodes = doc.DocumentNode.SelectNodes("//script");
                if (scriptNodes != null)
                {
                    var mp3Regex = new Regex(@"mp3\s*:\s*""(https?://[^""]+\.mp3)""");
                    foreach (var script in scriptNodes)
                    {
                        var match = mp3Regex.Match(script.InnerText);
                        if (match.Success)
                        {
                            currentAudioUrl = match.Groups[1].Value;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(currentAudioUrl))
                {
                    button_Download.Enabled = true;
                    button_Download.Text = "��������";
                }
                else
                {
                    button_Download.Enabled = false;
                    button_Download.Text = "����Ƶ�ļ�";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������������ʧ�ܣ�{ex.Message}");
                richTextBox_Content.Text = $"����ʧ�ܣ�{ex.Message}";
                button_Download.Text = "����ʧ��";
            }
        }

        // ==================== ���������� ====================
        // ʹ��HttpClient��������б�Ҫ������ͷ��ģ�������
        private async void button_Download_Click(object sender, EventArgs e)
        {
            if (!button_Download.Enabled || string.IsNullOrWhiteSpace(currentAudioUrl))
            {
                MessageBox.Show("��ǰ״̬�޷����أ���ѡ��һƪ������Ƶ�����¡�");
                return;
            }

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "��ѡ�񱣴��ı�����Ƶ���ļ���";
                if (fbd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return;
                }

                button_Download.Enabled = false;
                button_Download.Text = "������...";

                try
                {
                    // --- 1. �����ı��ĵ� ---
                    string title = MakeValidFileName(listBox_Articles.SelectedItem.ToString());
                    string textPath = Path.Combine(fbd.SelectedPath, $"{title}.txt");
                    using (StreamWriter writer = new StreamWriter(textPath, false, Encoding.UTF8))
                    {
                        await writer.WriteAsync(richTextBox_Content.Text);
                    }

                    // --- 2. ������Ƶ�ļ� ---
                    string audioPath = Path.Combine(fbd.SelectedPath, $"{title}.mp3");

                    // ����һ��������Ϣ���Ա����ǿ����Զ�����������
                    var request = new HttpRequestMessage(HttpMethod.Get, currentAudioUrl);

                    // ���Ĭ�ϱ�ͷ����Ӵ���������Ƶľ�ȷ��ͷ
                    request.Headers.Clear();
                    request.Headers.Add("Accept", "*/*");
                    request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");

                    // Referer������Ҫ�ģ���Ӧ��������ҳ�棬������mp3�ļ�����
                    request.Headers.Add("Referer", currentArticleUrl);

                    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36");
                    request.Headers.Add("Sec-Fetch-Dest", "video");
                    request.Headers.Add("Sec-Fetch-Mode", "no-cors");
                    request.Headers.Add("Sec-Fetch-Site", "same-origin");
                    request.Headers.Add("sec-ch-ua", "\"Not)A;Brand\";v=\"8\", \"Chromium\";v=\"138\", \"Google Chrome\";v=\"138\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");

                    // ���Ͷ��Ƶ�����
                    HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    // �����ķ�ʽд���ļ����Դ�����ļ�
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(audioPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    MessageBox.Show($"������ɣ�\n\n�ı�������: {textPath}\n��Ƶ������: {audioPath}", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"����ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    button_Download.Enabled = true;
                    button_Download.Text = "��������";
                }
            }
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(name, invalidRegStr, "_");
        }
    }
}
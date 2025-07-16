using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VoaDownloaderWpf
{
    public class VoaScraperService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string BaseUrl = "https://www.21voa.com/";

        public async Task<Dictionary<string, string>> LoadCategoriesAsync()
        {
            var categoryLinks = new Dictionary<string, string>();
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(BaseUrl);
            var categoryNodes = doc.DocumentNode.SelectNodes("//div[@id='lefter']/ul/li/a");

            if (categoryNodes != null)
            {
                foreach (var node in categoryNodes)
                {
                    string name = WebUtility.HtmlDecode(node.InnerText.Trim());
                    string link = node.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(link) && link.Contains(".html"))
                    {
                        string fullLink = new System.Uri(new System.Uri(BaseUrl), link).ToString();
                        if (!categoryLinks.ContainsKey(name))
                        {
                            categoryLinks[name] = fullLink;
                        }
                    }
                }
            }
            return categoryLinks;
        }

        public async Task<Dictionary<string, string>> FetchArticlesAsync(string categoryUrl, int page)
        {
            var articleLinks = new Dictionary<string, string>();
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
                        string fullLink = new System.Uri(new System.Uri(BaseUrl), link).ToString();
                        if (!articleLinks.ContainsKey(title))
                        {
                            articleLinks[title] = fullLink;
                        }
                    }
                }
            }
            return articleLinks;
        }

        /// <summary>
        /// (已修正) 将音频文件下载到本地临时缓存目录
        /// </summary>
        /// <param name="audioUrl">音频文件的网络URL</param>
        /// <param name="fileName">要保存的文件名</param>
        /// <param name="refererUrl">必需的Referer头，即文章页面的URL</param>
        /// <returns>成功下载后的本地文件路径，如果失败则返回null</returns>
        public async Task<string> DownloadAudioToCacheAsync(string audioUrl, string fileName, string refererUrl)
        {
            try
            {
                string cacheDirectory = Path.Combine(Path.GetTempPath(), "VoaDownloaderWpfCache");
                Directory.CreateDirectory(cacheDirectory);

                string localFilePath = Path.Combine(cacheDirectory, fileName);

                if (File.Exists(localFilePath))
                {
                    return localFilePath;
                }

                // --- 核心修正：使用 HttpRequestMessage 来添加 Referer 头 ---
                var request = new HttpRequestMessage(HttpMethod.Get, audioUrl);
                request.Headers.Add("Referer", refererUrl);

                using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                // --------------------------------------------------------

                return localFilePath;
            }
            catch (Exception)
            {
                return null; // 下载失败
            }
        }


        // VoaScraperService.cs

        public async Task<(string content, string audioUrl)> GetArticleDetailsAsync(string articleUrl)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(articleUrl);
            var contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='content']");
            string content = contentNode != null ? WebUtility.HtmlDecode(contentNode.InnerText.Trim()) : null;

            // =============================================================
            // ====         【核心修正】清理多余的连续空行              ====
            // =============================================================
            if (!string.IsNullOrEmpty(content))
            {
                // 这个正则表达式会匹配3个或更多连续的换行符(包括\r\n, \n, \r)
                // 并将其统一替换为两个标准的Windows换行符(\r\n\r\n)。
                // 这确保了段落之间最多只有一个空行。
                content = Regex.Replace(content, @"(\r\n|\n|\r){3,}", "\r\n\r\n");
            }

            // ... (方法后面的部分保持不变) ...
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

        public async Task DownloadFileWithProgressAsync(string fileUrl, string destinationPath, string referer, IProgress<double> progress)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, fileUrl);
            request.Headers.Add("Referer", referer);

            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength;

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalBytesRead = 0L;
                    var buffer = new byte[8192];
                    var isMoreToRead = true;

                    do
                    {
                        var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;
                            if (totalBytes.HasValue)
                            {
                                progress.Report((double)totalBytesRead / totalBytes.Value * 100.0);
                            }
                        }
                    }
                    while (isMoreToRead);
                }
            }
        }

        public string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            return Regex.Replace(name, $"[{invalidChars}]+", "_");
        }
    }
}
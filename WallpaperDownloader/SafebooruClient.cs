// SafebooruClient.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WallpaperDownloader
{
    public class SafebooruClient : IBooruClient
    {
        private readonly HttpClient _httpClient;
        public string SiteName => "Safebooru";

        public SafebooruClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyMoeDownloader/1.0");
        }

        public async Task<List<BooruImage>> GetImagesAsync(int page = 1, int limit = 40)
        {
            // Safebooru 使用不同的 API 端点和参数
            string apiUrl = $"https://safebooru.org/index.php?page=dapi&s=post&q=index&json=1&limit={limit}&pid={page - 1}";
            var response = await _httpClient.GetStringAsync(apiUrl);

            // Safebooru 的 JSON 格式与 Konachan 不同，需要一个专门的类来解析
            var safebooruPosts = JsonConvert.DeserializeObject<List<SafebooruPost>>(response);

            // 将 Safebooru 的数据格式转换为我们通用的 BooruImage 格式
            return safebooruPosts.Select(post => new BooruImage
            {
                Id = post.Id,
                Tags = post.Tags,
                CreatedAt = 0, // Safebooru API 不直接提供 Unix 时间戳，暂设为0
                FileSize = post.FileSize,
                FileURL = $"https://safebooru.org/images/{post.Directory}/{post.Image}",
                PreviewURL = $"https://safebooru.org/thumbnails/{post.Directory}/thumbnail_{post.Image}",
                SampleURL = $"https://safebooru.org/images/{post.Directory}/{post.Image}", // Safebooru 没有单独的 sample URL
                Width = post.Width,
                Height = post.Height
            }).ToList();
        }
    }

    // 用于解析 Safebooru 返回的独特 JSON 结构的辅助类
    public class SafebooruPost
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        [JsonProperty("directory")]
        public string Directory { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
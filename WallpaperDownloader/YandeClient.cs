// YandeClient.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WallpaperDownloader
{
    public class YandeClient : IBooruClient
    {
        private readonly HttpClient _httpClient;
        public string SiteName => "Yande.re";

        public YandeClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyMoeDownloader/1.0");
        }

        public async Task<List<BooruImage>> GetImagesAsync(int page = 1, int limit = 40)
        {
            // Yande.re 的 API 地址
            string apiUrl = $"https://yande.re/post.json?limit={limit}&page={page}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            return JsonConvert.DeserializeObject<List<BooruImage>>(response);
        }
    }
}
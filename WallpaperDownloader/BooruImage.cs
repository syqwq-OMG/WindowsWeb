// BooruImage.cs
using Newtonsoft.Json;
namespace WallpaperDownloader
{
    public class BooruImage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("file_size")]
        public long FileSize { get; set; }

        // 原图链接
        [JsonProperty("file_url")]
        public string FileURL { get; set; }

        // 预览图链接
        [JsonProperty("preview_url")]
        public string PreviewURL { get; set; }

        // 实际尺寸的图链接 (通常比预览图大，比原图小)
        [JsonProperty("sample_url")]
        public string SampleURL { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        // 用于显示的简短信息
        public string GetShortInfo()
        {
            return $"ID: {Id}\n尺寸: {Width}x{Height}\n标签: {string.Join(" ", Tags.Split(' ').Take(10))}...";
        }
    }
}

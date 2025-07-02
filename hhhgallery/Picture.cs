using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace hhhgallery
{
    public class Picture
    {
        // 这些属性将直接从JSON文件映射
        [JsonPropertyName("FileName")]
        public string FileName { get; set; }

        [JsonPropertyName("Category")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PictureCategory Category { get; set; }

        [JsonPropertyName("Author")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PictureAuthor Author { get; set; }

        // 辅助属性，用于获取图片的完整路径
        [JsonIgnore] // 告诉序列化器忽略此属性
        public string FullPath
        {
            get
            {
                // 构建相对于程序执行目录的完整路径
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", FileName);
            }
        }
    }
}

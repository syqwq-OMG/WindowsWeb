using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace hhhgallery
{
    internal class PictureManager
    {
        public List<Picture> AllPictures { get; private set; }

        public PictureManager()
        {
            AllPictures = new List<Picture>();
        }

        // 从JSON文件加载图片元数据
        public void LoadPictures()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "metadata.json");
                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show("错误: metadata.json 文件未找到!", "文件未找到", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string jsonString = File.ReadAllText(jsonPath);

                // 反序列化JSON数组到Picture对象列表
                var pictures = JsonSerializer.Deserialize<List<Picture>>(jsonString);

                if (pictures != null)
                {
                    // 验证每个图片文件是否存在
                    AllPictures = pictures.Where(p =>
                    {
                        if (File.Exists(p.FullPath))
                        {
                            return true;
                        }
                        else
                        {
                            // 如果文件不存在，可以选择记录日志或通知用户
                            Console.WriteLine($"警告: 图片文件 '{p.FileName}' 未在src目录中找到。");
                            return false;
                        }
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片数据时发生错误: {ex.Message}", "加载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 根据选择的类别和作者筛选图片
        public List<Picture> FilterPictures(PictureCategory category, PictureAuthor author)
        {
            IEnumerable<Picture> filteredList = AllPictures;

            // 如果选择的不是 "All"，则应用类别筛选
            if (category != PictureCategory.All)
            {
                filteredList = filteredList.Where(p => p.Category == category);
            }

            // 如果选择的不是 "All"，则应用作者筛选
            if (author != PictureAuthor.All)
            {
                filteredList = filteredList.Where(p => p.Author == author);
            }

            return filteredList.ToList();
        }
    }
}

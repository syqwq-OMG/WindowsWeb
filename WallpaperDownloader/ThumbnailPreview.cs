// ThumbnailPreview.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WallpaperDownloader
{
    public partial class ThumbnailPreview : UserControl
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public BooruImage ImageInfo { get; private set; }

        // 提供一个属性来直接访问和设置复选框的选中状态
        public bool IsSelected
        {
            get => selectCheckBox.Checked;
            set => selectCheckBox.Checked = value;
        }

        public ThumbnailPreview()
        {
            InitializeComponent();
            this.Margin = new Padding(5); // 给每个控件之间留出空隙
        }

        public void SetImageInfo(BooruImage imageInfo, ToolTip toolTip)
        {
            ImageInfo = imageInfo;
            // 使用 ToolTip 控件显示详细信息
            toolTip.SetToolTip(this.pictureBox, imageInfo.GetShortInfo());
            toolTip.SetToolTip(this.selectCheckBox, imageInfo.GetShortInfo());
        }

        public async Task LoadThumbnailAsync()
        {
            try
            {
                var imageData = await _httpClient.GetByteArrayAsync(ImageInfo.PreviewURL);
                using (var ms = new System.IO.MemoryStream(imageData))
                {
                    pictureBox.Image = System.Drawing.Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                // 如果加载失败，可以显示一个错误图标
                pictureBox.Image = pictureBox.ErrorImage;
                Console.WriteLine($"加载缩略图失败: {ImageInfo.PreviewURL} - {ex.Message}");
            }
        }
    }
}
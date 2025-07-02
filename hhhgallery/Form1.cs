namespace hhhgallery
{
    public partial class Form1 : Form
    {
        private PictureManager _pictureManager;

        public Form1()
        {
            InitializeComponent();
            _pictureManager = new PictureManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 加载图片数据
            _pictureManager.LoadPictures();

            // 填充ComboBox
            PopulateComboBoxes();

            // 初始加载所有图片
            DisplayPictures(_pictureManager.AllPictures);
        }

        private void PopulateComboBoxes()
        {
            // 使用 Enum.GetValues 填充下拉框
            cmbCategory.DataSource = Enum.GetValues(typeof(PictureCategory));
            cmbAuthor.DataSource = Enum.GetValues(typeof(PictureAuthor));

            // 默认选中 "All"
            cmbCategory.SelectedItem = PictureCategory.All;
            cmbAuthor.SelectedItem = PictureAuthor.All;
        }

        private void DisplayPictures(List<Picture> pictures)
        {
            // 清空旧的缩略图
            pnlThumbnails.Controls.Clear();

            foreach (var picture in pictures)
            {
                PictureBox pb = new PictureBox
                {
                    Size = new Size(150, 150),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    ImageLocation = picture.FullPath,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(5),
                    // 使用Tag属性存储整个Picture对象，方便后续使用
                    Tag = picture
                };

                pb.Click += PictureBox_Click;
                pnlThumbnails.Controls.Add(pb);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // 从ComboBox获取筛选条件
            PictureCategory selectedCategory = (PictureCategory)cmbCategory.SelectedItem;
            PictureAuthor selectedAuthor = (PictureAuthor)cmbAuthor.SelectedItem;

            // 调用Manager进行筛选
            var filteredPictures = _pictureManager.FilterPictures(selectedCategory, selectedAuthor);

            // 显示筛选结果
            DisplayPictures(filteredPictures);
        }

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb.Tag is Picture picture)
            {
                // 创建并显示预览窗体
                PreviewForm previewForm = new PreviewForm(picture);
                previewForm.ShowDialog();
            }
        }
    }
}

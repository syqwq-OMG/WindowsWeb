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
            // ����ͼƬ����
            _pictureManager.LoadPictures();

            // ���ComboBox
            PopulateComboBoxes();

            // ��ʼ��������ͼƬ
            DisplayPictures(_pictureManager.AllPictures);
        }

        private void PopulateComboBoxes()
        {
            // ʹ�� Enum.GetValues ���������
            cmbCategory.DataSource = Enum.GetValues(typeof(PictureCategory));
            cmbAuthor.DataSource = Enum.GetValues(typeof(PictureAuthor));

            // Ĭ��ѡ�� "All"
            cmbCategory.SelectedItem = PictureCategory.All;
            cmbAuthor.SelectedItem = PictureAuthor.All;
        }

        private void DisplayPictures(List<Picture> pictures)
        {
            // ��վɵ�����ͼ
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
                    // ʹ��Tag���Դ洢����Picture���󣬷������ʹ��
                    Tag = picture
                };

                pb.Click += PictureBox_Click;
                pnlThumbnails.Controls.Add(pb);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // ��ComboBox��ȡɸѡ����
            PictureCategory selectedCategory = (PictureCategory)cmbCategory.SelectedItem;
            PictureAuthor selectedAuthor = (PictureAuthor)cmbAuthor.SelectedItem;

            // ����Manager����ɸѡ
            var filteredPictures = _pictureManager.FilterPictures(selectedCategory, selectedAuthor);

            // ��ʾɸѡ���
            DisplayPictures(filteredPictures);
        }

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb.Tag is Picture picture)
            {
                // ��������ʾԤ������
                PreviewForm previewForm = new PreviewForm(picture);
                previewForm.ShowDialog();
            }
        }
    }
}

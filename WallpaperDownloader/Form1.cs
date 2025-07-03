// Form1.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WallpaperDownloader
{
    public partial class Form1 : Form
    {
        private Dictionary<string, IBooruClient> _clients;
        private CancellationTokenSource _cancellationTokenSource;
        private int _currentPage = 1;
        private bool _isLoading = false;
        private string _currentSite = "";

        public Form1()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // ��ʼ��������վ�ͻ��ˣ������� Yande.re
            _clients = new Dictionary<string, IBooruClient>
            {
                { "Konachan", new KonachanClient() },
                { "Yande.re", new YandeClient() },
                { "Safebooru", new SafebooruClient() }  // <-- �����һ��
            };

            // ��� ComboBox
            siteComboBox.Items.AddRange(_clients.Keys.ToArray());
            if (siteComboBox.Items.Count > 0)
            {
                siteComboBox.SelectedIndex = 0;
            }

            // ��ʼ����ͣ��ťΪ���ɼ�
            pauseButton.Visible = false;
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            if (siteComboBox.SelectedItem == null) return;

            // ����л�����վ��������
            string selectedSite = siteComboBox.SelectedItem.ToString();
            if (_currentSite != selectedSite)
            {
                _currentSite = selectedSite;
                _currentPage = 1;
                thumbnailFlowPanel.Controls.Clear();
                selectAllCheckBox.Checked = false;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            await LoadThumbnailsLoop(_cancellationTokenSource.Token);
        }

        private async Task LoadThumbnailsLoop(CancellationToken token)
        {
            _isLoading = true;
            UpdateUIState(); // ����UI״̬�����ü��ذ�ť����ʾ��ͣ��ť

            IBooruClient client = _clients[_currentSite];
            statusLabel.Text = $"���ڴ� {_currentSite} ����ͼƬ...";

            try
            {
                // ѭ�����أ�ֱ��û����ͼƬ��ȡ��
                while (!token.IsCancellationRequested)
                {
                    statusLabel.Text = $"���ڴ� {_currentSite} ���ص� {_currentPage} ҳ...";
                    List<BooruImage> images = await client.GetImagesAsync(page: _currentPage);
                    if (images == null || !images.Any())
                    {
                        statusLabel.Text = "�Ѽ�������ͼƬ��";
                        break; // û�и���ͼƬ��
                    }

                    foreach (var imgInfo in images)
                    {
                        if (token.IsCancellationRequested) break; // ÿ��ѭ��ǰ����Ƿ�������ȡ��
                        var thumbControl = new ThumbnailPreview();
                        thumbControl.SetImageInfo(imgInfo, this.imageToolTip);
                        thumbnailFlowPanel.Controls.Add(thumbControl);
                        await thumbControl.LoadThumbnailAsync();
                    }
                    _currentPage++; // ׼��������һҳ
                }
            }
            catch (OperationCanceledException)
            {
                statusLabel.Text = "��������ͣ��";
            }
            catch (Exception ex)
            {
                statusLabel.Text = "����ʧ��!";
                MessageBox.Show($"��ȡͼƬ�б�ʱ��������: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
                UpdateUIState(); // �ָ�UI״̬
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                statusLabel.Text = "������ͣ...";
            }
        }

        private void UpdateUIState()
        {
            loadButton.Enabled = !_isLoading;
            pauseButton.Visible = _isLoading;
            siteComboBox.Enabled = !_isLoading;
            downloadButton.Enabled = !_isLoading;
            browseButton.Enabled = !_isLoading;

            if (_isLoading)
            {
                loadButton.Text = "������...";
            }
            else
            {
                loadButton.Text = "����ͼƬ";
                if (thumbnailFlowPanel.Controls.Count > 0)
                {
                    loadButton.Text = "���ظ���";
                }
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            string savePath = pathTextBox.Text;
            if (string.IsNullOrEmpty(savePath))
            {
                MessageBox.Show("����ѡ�񱣴�·��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedThumbs = thumbnailFlowPanel.Controls.OfType<ThumbnailPreview>().Where(t => t.IsSelected).ToList();
            if (!selectedThumbs.Any())
            {
                MessageBox.Show("������ѡ��һ��ͼƬ!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- ��������������ʼ�� ---
            downloadProgressBar.Maximum = selectedThumbs.Count;
            downloadProgressBar.Value = 0;
            downloadProgressBar.Visible = true;
            // -------------------------

            downloadButton.Enabled = false;
            loadButton.Enabled = false;
            int successCount = 0;
            
            try
            {
                using (var client = new HttpClient())
                {
                    for (int i = 0; i < selectedThumbs.Count; i++)
                    {
                        var thumb = selectedThumbs[i];
                        statusLabel.Text = $"�������� ({i + 1}/{selectedThumbs.Count}): {thumb.ImageInfo.Id}.jpg";
                        try
                        {
                            var imageData = await client.GetByteArrayAsync(thumb.ImageInfo.FileURL);
                            string fileName = $"{thumb.ImageInfo.Id}.jpg";
                            string filePath = System.IO.Path.Combine(savePath, fileName);
                            System.IO.File.WriteAllBytes(filePath, imageData);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"����ʧ��: {thumb.ImageInfo.FileURL} - {ex.Message}");
                        }
                        
                        // --- ���������½����� ---
                        downloadProgressBar.Value = i + 1;
                        // -------------------------
                    }
                }
            }
            finally
            {
                // --- �޸ģ����ؽ������� ---
                statusLabel.Text = $"������ɣ��ɹ� {successCount} / {selectedThumbs.Count} �š�";
                MessageBox.Show($"������ɣ��ɹ� {successCount} �ţ�ʧ�� {selectedThumbs.Count - successCount} �š�", "���");
                
                // �ӳ�һС��ʱ������ؽ����������û��ܿ���100%��״̬
                await Task.Delay(1000); 
                downloadProgressBar.Visible = false;

                downloadButton.Enabled = true;
                loadButton.Enabled = true;
                // -------------------------
            }
        }

        private void selectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = selectAllCheckBox.Checked;
            foreach (ThumbnailPreview thumb in thumbnailFlowPanel.Controls.OfType<ThumbnailPreview>())
            {
                thumb.IsSelected = isChecked;
            }
        }
    }
}
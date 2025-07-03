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

            // 初始化所有网站客户端，并新增 Yande.re
            _clients = new Dictionary<string, IBooruClient>
            {
                { "Konachan", new KonachanClient() },
                { "Yande.re", new YandeClient() },
                { "Safebooru", new SafebooruClient() }  // <-- 添加这一行
            };

            // 填充 ComboBox
            siteComboBox.Items.AddRange(_clients.Keys.ToArray());
            if (siteComboBox.Items.Count > 0)
            {
                siteComboBox.SelectedIndex = 0;
            }

            // 初始化暂停按钮为不可见
            pauseButton.Visible = false;
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            if (siteComboBox.SelectedItem == null) return;

            // 如果切换了网站，则重置
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
            UpdateUIState(); // 更新UI状态：禁用加载按钮，显示暂停按钮

            IBooruClient client = _clients[_currentSite];
            statusLabel.Text = $"正在从 {_currentSite} 加载图片...";

            try
            {
                // 循环加载，直到没有新图片或被取消
                while (!token.IsCancellationRequested)
                {
                    statusLabel.Text = $"正在从 {_currentSite} 加载第 {_currentPage} 页...";
                    List<BooruImage> images = await client.GetImagesAsync(page: _currentPage);
                    if (images == null || !images.Any())
                    {
                        statusLabel.Text = "已加载所有图片。";
                        break; // 没有更多图片了
                    }

                    foreach (var imgInfo in images)
                    {
                        if (token.IsCancellationRequested) break; // 每次循环前检查是否已请求取消
                        var thumbControl = new ThumbnailPreview();
                        thumbControl.SetImageInfo(imgInfo, this.imageToolTip);
                        thumbnailFlowPanel.Controls.Add(thumbControl);
                        await thumbControl.LoadThumbnailAsync();
                    }
                    _currentPage++; // 准备加载下一页
                }
            }
            catch (OperationCanceledException)
            {
                statusLabel.Text = "加载已暂停。";
            }
            catch (Exception ex)
            {
                statusLabel.Text = "加载失败!";
                MessageBox.Show($"获取图片列表时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
                UpdateUIState(); // 恢复UI状态
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                statusLabel.Text = "正在暂停...";
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
                loadButton.Text = "加载中...";
            }
            else
            {
                loadButton.Text = "加载图片";
                if (thumbnailFlowPanel.Controls.Count > 0)
                {
                    loadButton.Text = "加载更多";
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
                MessageBox.Show("请先选择保存路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedThumbs = thumbnailFlowPanel.Controls.OfType<ThumbnailPreview>().Where(t => t.IsSelected).ToList();
            if (!selectedThumbs.Any())
            {
                MessageBox.Show("请至少选择一张图片!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- 新增：进度条初始化 ---
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
                        statusLabel.Text = $"正在下载 ({i + 1}/{selectedThumbs.Count}): {thumb.ImageInfo.Id}.jpg";
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
                            Console.WriteLine($"下载失败: {thumb.ImageInfo.FileURL} - {ex.Message}");
                        }
                        
                        // --- 新增：更新进度条 ---
                        downloadProgressBar.Value = i + 1;
                        // -------------------------
                    }
                }
            }
            finally
            {
                // --- 修改：下载结束后处理 ---
                statusLabel.Text = $"下载完成！成功 {successCount} / {selectedThumbs.Count} 张。";
                MessageBox.Show($"下载完成！成功 {successCount} 张，失败 {selectedThumbs.Count - successCount} 张。", "完成");
                
                // 延迟一小段时间后隐藏进度条，让用户能看到100%的状态
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
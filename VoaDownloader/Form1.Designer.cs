namespace VoaDownloader
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            comboBox_Category = new ComboBox();
            label_Category = new Label();
            label_Page = new Label();
            button_Fetch = new Button();
            button_Download = new Button();
            statusStrip1 = new StatusStrip();
            label_Status = new ToolStripStatusLabel();
            progressBar = new ToolStripProgressBar();
            numericUpDown_Page = new NumericUpDown();
            splitContainer1 = new SplitContainer();
            checkedListBox_Articles = new CheckedListBox();
            checkBox_SelectAll = new CheckBox();
            label_ArticleList = new Label();
            richTextBox_Content = new RichTextBox();
            label_ContentPreview = new Label();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Page).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox_Category
            // 
            comboBox_Category.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Category.FormattingEnabled = true;
            comboBox_Category.Location = new Point(129, 20);
            comboBox_Category.Margin = new Padding(4, 5, 4, 5);
            comboBox_Category.Name = "comboBox_Category";
            comboBox_Category.Size = new Size(373, 28);
            comboBox_Category.TabIndex = 0;
            // 
            // label_Category
            // 
            label_Category.AutoSize = true;
            label_Category.Location = new Point(22, 25);
            label_Category.Margin = new Padding(4, 0, 4, 0);
            label_Category.Name = "label_Category";
            label_Category.Size = new Size(84, 20);
            label_Category.TabIndex = 1;
            label_Category.Text = "选择分类：";
            // 
            // label_Page
            // 
            label_Page.AutoSize = true;
            label_Page.Location = new Point(525, 25);
            label_Page.Margin = new Padding(4, 0, 4, 0);
            label_Page.Name = "label_Page";
            label_Page.Size = new Size(54, 20);
            label_Page.TabIndex = 5;
            label_Page.Text = "页码：";
            // 
            // button_Fetch
            // 
            button_Fetch.Location = new Point(710, 17);
            button_Fetch.Margin = new Padding(4, 5, 4, 5);
            button_Fetch.Name = "button_Fetch";
            button_Fetch.Size = new Size(135, 38);
            button_Fetch.TabIndex = 6;
            button_Fetch.Text = "获取列表";
            button_Fetch.UseVisualStyleBackColor = true;
            button_Fetch.Click += button_Fetch_Click;
            // 
            // button_Download
            // 
            button_Download.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_Download.Font = new Font("宋体", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button_Download.Location = new Point(401, 831);
            button_Download.Margin = new Padding(4, 5, 4, 5);
            button_Download.Name = "button_Download";
            button_Download.Size = new Size(218, 38);
            button_Download.TabIndex = 9;
            button_Download.Text = "下载选中项";
            button_Download.UseVisualStyleBackColor = true;
            button_Download.Click += button_Download_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { label_Status, progressBar });
            statusStrip1.Location = new Point(0, 891);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(2, 0, 21, 0);
            statusStrip1.Size = new Size(1026, 31);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // label_Status
            // 
            label_Status.Name = "label_Status";
            label_Status.Size = new Size(851, 25);
            label_Status.Spring = true;
            label_Status.Text = "就绪";
            label_Status.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(150, 23);
            // 
            // numericUpDown_Page
            // 
            numericUpDown_Page.Location = new Point(610, 20);
            numericUpDown_Page.Margin = new Padding(4, 5, 4, 5);
            numericUpDown_Page.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_Page.Name = "numericUpDown_Page";
            numericUpDown_Page.Size = new Size(75, 27);
            numericUpDown_Page.TabIndex = 14;
            numericUpDown_Page.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(18, 70);
            splitContainer1.Margin = new Padding(4, 5, 4, 5);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(checkedListBox_Articles);
            splitContainer1.Panel1.Controls.Add(checkBox_SelectAll);
            splitContainer1.Panel1.Controls.Add(label_ArticleList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(richTextBox_Content);
            splitContainer1.Panel2.Controls.Add(label_ContentPreview);
            splitContainer1.Size = new Size(990, 732);
            splitContainer1.SplitterDistance = 334;
            splitContainer1.SplitterWidth = 7;
            splitContainer1.TabIndex = 16;
            // 
            // checkedListBox_Articles
            // 
            checkedListBox_Articles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            checkedListBox_Articles.CheckOnClick = true;
            checkedListBox_Articles.FormattingEnabled = true;
            checkedListBox_Articles.Location = new Point(8, 45);
            checkedListBox_Articles.Margin = new Padding(4, 5, 4, 5);
            checkedListBox_Articles.Name = "checkedListBox_Articles";
            checkedListBox_Articles.Size = new Size(976, 268);
            checkedListBox_Articles.TabIndex = 16;
            checkedListBox_Articles.SelectedIndexChanged += checkedListBox_Articles_SelectedIndexChanged;
            // 
            // checkBox_SelectAll
            // 
            checkBox_SelectAll.AutoSize = true;
            checkBox_SelectAll.Location = new Point(111, 13);
            checkBox_SelectAll.Margin = new Padding(4, 5, 4, 5);
            checkBox_SelectAll.Name = "checkBox_SelectAll";
            checkBox_SelectAll.Size = new Size(97, 24);
            checkBox_SelectAll.TabIndex = 18;
            checkBox_SelectAll.Text = "全选/不选";
            checkBox_SelectAll.UseVisualStyleBackColor = true;
            checkBox_SelectAll.CheckedChanged += checkBox_SelectAll_CheckedChanged;
            // 
            // label_ArticleList
            // 
            label_ArticleList.AutoSize = true;
            label_ArticleList.Location = new Point(4, 13);
            label_ArticleList.Margin = new Padding(4, 0, 4, 0);
            label_ArticleList.Name = "label_ArticleList";
            label_ArticleList.Size = new Size(73, 20);
            label_ArticleList.TabIndex = 17;
            label_ArticleList.Text = "文章列表:";
            // 
            // richTextBox_Content
            // 
            richTextBox_Content.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox_Content.Location = new Point(4, 38);
            richTextBox_Content.Margin = new Padding(4, 5, 4, 5);
            richTextBox_Content.Name = "richTextBox_Content";
            richTextBox_Content.ReadOnly = true;
            richTextBox_Content.Size = new Size(976, 339);
            richTextBox_Content.TabIndex = 13;
            richTextBox_Content.Text = "";
            // 
            // label_ContentPreview
            // 
            label_ContentPreview.AutoSize = true;
            label_ContentPreview.Location = new Point(4, 13);
            label_ContentPreview.Margin = new Padding(4, 0, 4, 0);
            label_ContentPreview.Name = "label_ContentPreview";
            label_ContentPreview.Size = new Size(143, 20);
            label_ContentPreview.TabIndex = 12;
            label_ContentPreview.Text = "内容预览(单击条目):";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1026, 922);
            Controls.Add(splitContainer1);
            Controls.Add(numericUpDown_Page);
            Controls.Add(statusStrip1);
            Controls.Add(button_Download);
            Controls.Add(button_Fetch);
            Controls.Add(label_Page);
            Controls.Add(label_Category);
            Controls.Add(comboBox_Category);
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1041, 969);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "VOA 英语资料批量下载器";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Page).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Category;
        private System.Windows.Forms.Label label_Category;
        private System.Windows.Forms.Label label_Page;
        private System.Windows.Forms.Button button_Fetch;
        private System.Windows.Forms.Button button_Download;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel label_Status;
        private System.Windows.Forms.NumericUpDown numericUpDown_Page;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox checkedListBox_Articles;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.Label label_ArticleList;
        private System.Windows.Forms.RichTextBox richTextBox_Content;
        private System.Windows.Forms.Label label_ContentPreview;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
    }
}
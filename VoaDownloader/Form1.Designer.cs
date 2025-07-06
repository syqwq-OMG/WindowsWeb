namespace VoaDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox_Category = new System.Windows.Forms.ComboBox();
            this.label_Category = new System.Windows.Forms.Label();
            this.textBox_Page = new System.Windows.Forms.TextBox();
            this.label_Page = new System.Windows.Forms.Label();
            this.button_Fetch = new System.Windows.Forms.Button();
            this.listBox_Articles = new System.Windows.Forms.ListBox();
            this.richTextBox_Content = new System.Windows.Forms.RichTextBox();
            this.button_Download = new System.Windows.Forms.Button();
            this.label_ArticleList = new System.Windows.Forms.Label();
            this.label_ContentPreview = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox_Category
            // 
            this.comboBox_Category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Category.FormattingEnabled = true;
            this.comboBox_Category.Location = new System.Drawing.Point(86, 12);
            this.comboBox_Category.Name = "comboBox_Category";
            this.comboBox_Category.Size = new System.Drawing.Size(250, 20);
            this.comboBox_Category.TabIndex = 0;
            // 
            // label_Category
            // 
            this.label_Category.AutoSize = true;
            this.label_Category.Location = new System.Drawing.Point(15, 15);
            this.label_Category.Name = "label_Category";
            this.label_Category.Size = new System.Drawing.Size(65, 12);
            this.label_Category.TabIndex = 1;
            this.label_Category.Text = "选择分类：";
            // 
            // textBox_Page
            // 
            this.textBox_Page.Location = new System.Drawing.Point(407, 12);
            this.textBox_Page.Name = "textBox_Page";
            this.textBox_Page.Size = new System.Drawing.Size(50, 21);
            this.textBox_Page.TabIndex = 4;
            this.textBox_Page.Text = "1";
            // 
            // label_Page
            // 
            this.label_Page.AutoSize = true;
            this.label_Page.Location = new System.Drawing.Point(350, 15);
            this.label_Page.Name = "label_Page";
            this.label_Page.Size = new System.Drawing.Size(53, 12);
            this.label_Page.TabIndex = 5;
            this.label_Page.Text = "页码：";
            // 
            // button_Fetch
            // 
            this.button_Fetch.Location = new System.Drawing.Point(473, 10);
            this.button_Fetch.Name = "button_Fetch";
            this.button_Fetch.Size = new System.Drawing.Size(90, 23);
            this.button_Fetch.TabIndex = 6;
            this.button_Fetch.Text = "获取列表";
            this.button_Fetch.UseVisualStyleBackColor = true;
            this.button_Fetch.Click += new System.EventHandler(this.button_Fetch_Click);
            // 
            // listBox_Articles
            // 
            this.listBox_Articles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Articles.FormattingEnabled = true;
            this.listBox_Articles.ItemHeight = 12;
            this.listBox_Articles.Location = new System.Drawing.Point(17, 65);
            this.listBox_Articles.Name = "listBox_Articles";
            this.listBox_Articles.Size = new System.Drawing.Size(655, 172);
            this.listBox_Articles.TabIndex = 7;
            this.listBox_Articles.SelectedIndexChanged += new System.EventHandler(this.listBox_Articles_SelectedIndexChanged);
            // 
            // richTextBox_Content
            // 
            this.richTextBox_Content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Content.Location = new System.Drawing.Point(17, 269);
            this.richTextBox_Content.Name = "richTextBox_Content";
            this.richTextBox_Content.ReadOnly = true;
            this.richTextBox_Content.Size = new System.Drawing.Size(655, 230);
            this.richTextBox_Content.TabIndex = 8;
            this.richTextBox_Content.Text = "";
            // 
            // button_Download
            // 
            this.button_Download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Download.Enabled = false;
            this.button_Download.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Download.Location = new System.Drawing.Point(557, 505);
            this.button_Download.Name = "button_Download";
            this.button_Download.Size = new System.Drawing.Size(115, 35);
            this.button_Download.TabIndex = 9;
            this.button_Download.Text = "下载内容";
            this.button_Download.UseVisualStyleBackColor = true;
            this.button_Download.Click += new System.EventHandler(this.button_Download_Click);
            // 
            // label_ArticleList
            // 
            this.label_ArticleList.AutoSize = true;
            this.label_ArticleList.Location = new System.Drawing.Point(15, 50);
            this.label_ArticleList.Name = "label_ArticleList";
            this.label_ArticleList.Size = new System.Drawing.Size(65, 12);
            this.label_ArticleList.TabIndex = 10;
            this.label_ArticleList.Text = "文章列表:";
            // 
            // label_ContentPreview
            // 
            this.label_ContentPreview.AutoSize = true;
            this.label_ContentPreview.Location = new System.Drawing.Point(15, 254);
            this.label_ContentPreview.Name = "label_ContentPreview";
            this.label_ContentPreview.Size = new System.Drawing.Size(65, 12);
            this.label_ContentPreview.TabIndex = 11;
            this.label_ContentPreview.Text = "内容预览:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 551);
            this.Controls.Add(this.label_ContentPreview);
            this.Controls.Add(this.label_ArticleList);
            this.Controls.Add(this.button_Download);
            this.Controls.Add(this.richTextBox_Content);
            this.Controls.Add(this.listBox_Articles);
            this.Controls.Add(this.button_Fetch);
            this.Controls.Add(this.label_Page);
            this.Controls.Add(this.textBox_Page);
            this.Controls.Add(this.label_Category);
            this.Controls.Add(this.comboBox_Category);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VOA 英语资料下载器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Category;
        private System.Windows.Forms.Label label_Category;
        private System.Windows.Forms.TextBox textBox_Page;
        private System.Windows.Forms.Label label_Page;
        private System.Windows.Forms.Button button_Fetch;
        private System.Windows.Forms.ListBox listBox_Articles;
        private System.Windows.Forms.RichTextBox richTextBox_Content;
        private System.Windows.Forms.Button button_Download;
        private System.Windows.Forms.Label label_ArticleList;
        private System.Windows.Forms.Label label_ContentPreview;
    }
}
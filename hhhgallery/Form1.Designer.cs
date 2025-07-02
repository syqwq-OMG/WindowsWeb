namespace hhhgallery
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
            this.pnlTopControls = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.cmbAuthor = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.pnlThumbnails = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlTopControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopControls
            // 
            this.pnlTopControls.Controls.Add(this.lblCategory);
            this.pnlTopControls.Controls.Add(this.cmbCategory);
            this.pnlTopControls.Controls.Add(this.lblAuthor);
            this.pnlTopControls.Controls.Add(this.cmbAuthor);
            this.pnlTopControls.Controls.Add(this.btnFilter);
            this.pnlTopControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopControls.Location = new System.Drawing.Point(0, 0);
            this.pnlTopControls.Name = "pnlTopControls";
            this.pnlTopControls.Padding = new System.Windows.Forms.Padding(10);
            this.pnlTopControls.Size = new System.Drawing.Size(1048, 55);
            this.pnlTopControls.TabIndex = 0;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(13, 15);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(43, 20);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "类别:";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(62, 13);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(150, 28);
            this.cmbCategory.TabIndex = 1;
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(225, 15);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(43, 20);
            this.lblAuthor.TabIndex = 2;
            this.lblAuthor.Text = "作者:";
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbAuthor
            // 
            this.cmbAuthor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthor.FormattingEnabled = true;
            this.cmbAuthor.Location = new System.Drawing.Point(274, 13);
            this.cmbAuthor.Name = "cmbAuthor";
            this.cmbAuthor.Size = new System.Drawing.Size(150, 28);
            this.cmbAuthor.TabIndex = 3;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(437, 13);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(94, 29);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "筛选";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // pnlThumbnails
            // 
            this.pnlThumbnails.AutoScroll = true;
            this.pnlThumbnails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumbnails.Location = new System.Drawing.Point(0, 55);
            this.pnlThumbnails.Name = "pnlThumbnails";
            this.pnlThumbnails.Padding = new System.Windows.Forms.Padding(10);
            this.pnlThumbnails.Size = new System.Drawing.Size(1048, 642);
            this.pnlThumbnails.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 697);
            this.Controls.Add(this.pnlThumbnails);
            this.Controls.Add(this.pnlTopControls);
            this.Name = "Form1";
            this.Text = "我的画廊";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlTopControls.ResumeLayout(false);
            this.pnlTopControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel pnlTopControls;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblAuthor;
        private ComboBox cmbAuthor;
        private Button btnFilter;
        private FlowLayoutPanel pnlThumbnails;
    }
}

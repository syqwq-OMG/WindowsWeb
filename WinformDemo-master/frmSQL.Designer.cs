namespace myproject
{
    partial class frmSQL
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
            components = new System.ComponentModel.Container();
            btnLink = new Button();
            btnInsert = new Button();
            dataGridView1 = new DataGridView();
            cboMajor = new ComboBox();
            txtStuName = new TextBox();
            btnSearch = new Button();
            label1 = new Label();
            label2 = new Label();
            chkAll = new CheckBox();
            lblCount = new Label();
            btnAlert = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            listView1 = new ListView();
            listView2 = new ListView();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            tbxName = new TextBox();
            tbxSID = new TextBox();
            tbxGender = new TextBox();
            ttbxIntro = new RichTextBox();
            btnUpdate = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            btnGraph = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnLink
            // 
            btnLink.Location = new Point(50, 48);
            btnLink.Margin = new Padding(4);
            btnLink.Name = "btnLink";
            btnLink.Size = new Size(157, 27);
            btnLink.TabIndex = 0;
            btnLink.Text = "测试数据库链接";
            btnLink.UseVisualStyleBackColor = true;
            btnLink.Click += btnLink_Click;
            // 
            // btnInsert
            // 
            btnInsert.Location = new Point(260, 48);
            btnInsert.Margin = new Padding(4);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(157, 27);
            btnInsert.TabIndex = 1;
            btnInsert.Text = "一键打卡";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(50, 208);
            dataGridView1.Margin = new Padding(4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(483, 514);
            dataGridView1.TabIndex = 2;
            // 
            // cboMajor
            // 
            cboMajor.FormattingEnabled = true;
            cboMajor.Location = new Point(99, 99);
            cboMajor.Margin = new Padding(4);
            cboMajor.Name = "cboMajor";
            cboMajor.Size = new Size(203, 28);
            cboMajor.TabIndex = 3;
            // 
            // txtStuName
            // 
            txtStuName.Location = new Point(99, 135);
            txtStuName.Margin = new Padding(4);
            txtStuName.Name = "txtStuName";
            txtStuName.Size = new Size(203, 27);
            txtStuName.TabIndex = 4;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(311, 135);
            btnSearch.Margin = new Padding(4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(105, 27);
            btnSearch.TabIndex = 5;
            btnSearch.Text = "查找";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(50, 139);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(39, 20);
            label1.TabIndex = 6;
            label1.Text = "姓名";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(50, 99);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(39, 20);
            label2.TabIndex = 7;
            label2.Text = "专业";
            // 
            // chkAll
            // 
            chkAll.AutoSize = true;
            chkAll.Location = new Point(351, 101);
            chkAll.Margin = new Padding(4);
            chkAll.Name = "chkAll";
            chkAll.Size = new Size(61, 24);
            chkAll.TabIndex = 8;
            chkAll.Text = "全部";
            chkAll.UseVisualStyleBackColor = true;
            chkAll.CheckedChanged += chkAll_CheckedChanged;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Location = new Point(50, 185);
            lblCount.Margin = new Padding(4, 0, 4, 0);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(57, 20);
            lblCount.TabIndex = 9;
            lblCount.Text = "details";
            // 
            // btnAlert
            // 
            btnAlert.Location = new Point(1356, 37);
            btnAlert.Margin = new Padding(4);
            btnAlert.Name = "btnAlert";
            btnAlert.Size = new Size(329, 27);
            btnAlert.TabIndex = 10;
            btnAlert.Text = "开始报警";
            btnAlert.UseVisualStyleBackColor = true;
            btnAlert.Click += button1_Click_1;
            // 
            // timer1
            // 
            timer1.Interval = 3000;
            timer1.Tick += timer1_Tick;
            // 
            // listView1
            // 
            listView1.Location = new Point(1356, 72);
            listView1.Margin = new Padding(4);
            listView1.Name = "listView1";
            listView1.Size = new Size(120, 95);
            listView1.TabIndex = 11;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.List;
            // 
            // listView2
            // 
            listView2.Location = new Point(1523, 78);
            listView2.Margin = new Padding(4);
            listView2.Name = "listView2";
            listView2.Size = new Size(113, 81);
            listView2.TabIndex = 12;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = View.List;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(589, 19);
            label3.Name = "label3";
            label3.Size = new Size(39, 20);
            label3.TabIndex = 13;
            label3.Text = "姓名";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(589, 58);
            label4.Name = "label4";
            label4.Size = new Size(39, 20);
            label4.TabIndex = 14;
            label4.Text = "学号";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(589, 107);
            label5.Name = "label5";
            label5.Size = new Size(39, 20);
            label5.TabIndex = 15;
            label5.Text = "性别";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(841, 19);
            label6.Name = "label6";
            label6.Size = new Size(44, 20);
            label6.TabIndex = 16;
            label6.Text = "intro";
            // 
            // tbxName
            // 
            tbxName.Location = new Point(638, 18);
            tbxName.Name = "tbxName";
            tbxName.ReadOnly = true;
            tbxName.Size = new Size(133, 27);
            tbxName.TabIndex = 17;
            // 
            // tbxSID
            // 
            tbxSID.Location = new Point(638, 62);
            tbxSID.Name = "tbxSID";
            tbxSID.ReadOnly = true;
            tbxSID.Size = new Size(197, 27);
            tbxSID.TabIndex = 18;
            // 
            // tbxGender
            // 
            tbxGender.Location = new Point(638, 106);
            tbxGender.Name = "tbxGender";
            tbxGender.ReadOnly = true;
            tbxGender.Size = new Size(158, 27);
            tbxGender.TabIndex = 19;
            // 
            // ttbxIntro
            // 
            ttbxIntro.Location = new Point(891, 19);
            ttbxIntro.Name = "ttbxIntro";
            ttbxIntro.Size = new Size(234, 122);
            ttbxIntro.TabIndex = 21;
            ttbxIntro.Text = "";
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(1159, 29);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(124, 35);
            btnUpdate.TabIndex = 22;
            btnUpdate.Text = "更新intro";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click_1;
            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1.25F;
            formsPlot1.Location = new Point(601, 174);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(1066, 548);
            formsPlot1.TabIndex = 23;
            // 
            // btnGraph
            // 
            btnGraph.Location = new Point(1159, 85);
            btnGraph.Name = "btnGraph";
            btnGraph.Size = new Size(124, 42);
            btnGraph.TabIndex = 24;
            btnGraph.Text = "专业分布图像";
            btnGraph.UseVisualStyleBackColor = true;
            btnGraph.Click += btnGraph_Click;
            // 
            // frmSQL
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1972, 736);
            Controls.Add(btnGraph);
            Controls.Add(formsPlot1);
            Controls.Add(btnUpdate);
            Controls.Add(ttbxIntro);
            Controls.Add(tbxGender);
            Controls.Add(tbxSID);
            Controls.Add(tbxName);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(listView2);
            Controls.Add(listView1);
            Controls.Add(btnAlert);
            Controls.Add(lblCount);
            Controls.Add(chkAll);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSearch);
            Controls.Add(txtStuName);
            Controls.Add(cboMajor);
            Controls.Add(dataGridView1);
            Controls.Add(btnInsert);
            Controls.Add(btnLink);
            Margin = new Padding(4);
            Name = "frmSQL";
            Text = "SQL练习";
            Load += frmSQL_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLink;
        private Button btnInsert;
        private DataGridView dataGridView1;
        private ComboBox cboMajor;
        private TextBox txtStuName;
        private Button btnSearch;
        private Label label1;
        private Label label2;
        private CheckBox chkAll;
        private Label lblCount;
        private Button btnAlert;
        private System.Windows.Forms.Timer timer1;
        private ListView listView1;
        private ListView listView2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox tbxName;
        private TextBox tbxSID;
        private TextBox tbxGender;
        private RichTextBox ttbxIntro;
        private Button btnUpdate;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button btnGraph;
    }
}
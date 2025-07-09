using Microsoft.VisualBasic;
using SQL;
using System.Data;
using System.Data.SqlClient; // 需要引用这个命名空间来使用 SqlParameter
using System.Text;
using ScottPlot;

namespace myproject
{
    public partial class frmSQL : Form
    {
        string msg = string.Empty;
        SQLHelper sh;
        // **[修改]** 获取所有需要的字段，包括主键(studentNo)和需要编辑的字段(Intro)
        string base_sql = "select studentNo, studentName, Gender, Intro, Major from tblTopStudents where 1=1";

        /// <summary>
        /// 构造函数往往放置一些初始化的工作
        /// </summary>
        public frmSQL()
        {
            InitializeComponent();
            sh = new SQLHelper(); //数据库链接对象初始化
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            string sql = "select count(*) from tblstudents"; //该SQL意思是，获取tblstudents的行数
            try
            {
                string? num = sh.RunSelectSQLToScalar(sql);  //一般运行查询语句
                msg = string.Format("我们班共有{0}个同学!", num);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                sh.Close();
            }
            MessageBox.Show(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string studentnumber = Interaction.InputBox("打卡", "请输入打卡的学号，默认是自己", "10130212110");
            try
            {
                string sql = string.Format("insert into tblStudentAbsent(studentNumber)values('{0}')", studentnumber);
                int ret = sh.RunSQL(sql); //一般运行 查询之外的删、改、增
                msg = string.Format("打卡成功");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                sh.Close();
            }
            MessageBox.Show(msg);
        }

        private void frmSQL_Load(object sender, EventArgs e)
        {
            bindData(base_sql);  //把数据绑定到datagridview
            initCombox(); //初始化combobox
            // 初始化 ImageList
            ImageList imgList = new ImageList();
            imgList.ColorDepth = ColorDepth.Depth32Bit;
            imgList.ImageSize = new Size(32, 32);

            try
            {
                imgList.Images.Add(System.Drawing.Image.FromFile(@"..\..\..\faces\boy.jpg"));
                imgList.Images.Add(System.Drawing.Image.FromFile(@"..\..\..\faces\girl.jpg"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载图标失败: " + ex.Message);
            }

            listView2.SmallImageList = imgList;
            listView2.LargeImageList = imgList;

            // **[新增]** 将新添加的事件处理程序绑定到控件
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // 保持原始的 base_sql 不变，在此基础上构建查询
            string sql = "select studentNo, studentName, Gender, Intro, Major from tblTopStudents where 1=1";
            string major = cboMajor.Text;
            string name = txtStuName.Text;

            if (cboMajor.Text != "全部显示")
            {
                if (major.Length > 0)
                {
                    sql += string.Format(" and major='{0}'", major);
                }
            }
            // **[修改]** 使用 like 实现模糊查询
            if (!string.IsNullOrEmpty(name))
            {
                sql += string.Format(" and studentname like '%{0}%'", name);
            }

            bindData(sql);
        }

        private void bindData(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                sh.RunSQL(sql, ref ds);
                DataTable dt = ds.Tables[0];
                dataGridView1.DataSource = dt;

                // **[新增]** 自定义列标题并隐藏不需要在表格中显示的列
                if (dataGridView1.Columns["studentName"] != null)
                    dataGridView1.Columns["studentName"].HeaderText = "姓名";
                if (dataGridView1.Columns["Major"] != null)
                    dataGridView1.Columns["Major"].HeaderText = "专业";
                if (dataGridView1.Columns["studentNo"] != null)
                    dataGridView1.Columns["studentNo"].HeaderText = "学号";

                // 隐藏不希望用户直接在表格中看到的列
                if (dataGridView1.Columns["Gender"] != null)
                    dataGridView1.Columns["Gender"].Visible = false;
                if (dataGridView1.Columns["Intro"] != null)
                    dataGridView1.Columns["Intro"].Visible = false;

                lblCount.Text = string.Format("共有{0}个同学", dt.Rows.Count);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                MessageBox.Show(msg);
            }
            finally
            {
                sh.Close();
            }
        }

        private void initCombox()
        {
            string sql = "SELECT DISTINCT major FROM tblTopStudents";
            DataSet ds = new DataSet();
            try
            {
                sh.RunSQL(sql, ref ds);
                DataTable dt = ds.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["major"] = "全部显示";
                dt.Rows.InsertAt(newRow, 0);

                cboMajor.DataSource = dt;
                cboMajor.DisplayMember = "major";
                cboMajor.ValueMember = "major";
                cboMajor.Text = "全部显示";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                MessageBox.Show(msg);
            }
            finally
            {
                sh.Close();
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                cboMajor.Text = "全部显示";
                bindData(base_sql);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (timer1.Enabled)
                btnAlert.Text = "停止报警";
            else
                btnAlert.Text = "开始报警";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SearchStu();
        }

        private void SearchStu()
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            string sql = "SELECT studentno,STUDENTNAME,gender FROM tblTopStudents\r\nWHERE studentNo NOT IN\r\n(\r\nselect STUDENTNUMBER from tblStudentAbsent where DATEDIFF(DAY,dtedate,GETDATE())=0\r\n)\r\n";
            DataSet ds = new DataSet();
            try
            {
                sh.RunSQL(sql, ref ds);
                DataTable dt = ds.Tables[0];
                bindImgTolist(dt);
                foreach (DataRow stu in dt.Rows)
                {
                    StringBuilder tmp = new StringBuilder();
                    tmp.Append(string.Format("学号:{0},姓名:{1}", stu[0], stu[1]));
                    listView1.Items.Add(tmp.ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                MessageBox.Show(msg);
            }
            finally
            {
                sh.Close();
            }
        }

        private void bindImgTolist(DataTable dt)
        {
            foreach (DataRow stu in dt.Rows)
            {
                ListViewItem item1 = new ListViewItem(stu[1].ToString());
                if (stu[2].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) || stu[2].ToString() == "男")
                    item1.ImageIndex = 0;
                else
                    item1.ImageIndex = 1;
                listView2.Items.Add(item1);
            }
        }

        // **[新增]** DataGridView单元格点击事件处理
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 确保点击的不是表头
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 从隐藏列中安全地获取数据并填充
                tbxName.Text = row.Cells["studentName"].Value?.ToString();
                tbxSID.Text = row.Cells["studentNo"].Value?.ToString();
                ttbxIntro.Text = row.Cells["Intro"].Value?.ToString();

                // 处理性别（bit类型在C#中通常映射为bool）
                if (row.Cells["Gender"].Value != DBNull.Value)
                {
                    bool isMale = Convert.ToBoolean(row.Cells["Gender"].Value);
                    tbxGender.Text = isMale ? "男" : "女";
                }
                else
                {
                    tbxGender.Text = "未知";
                }
            }
        }

        // **[新增]** 更新按钮点击事件处理
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // 检查是否已选择学生（通过学号文本框是否有值来判断）
            if (string.IsNullOrEmpty(tbxSID.Text))
            {
                MessageBox.Show("请先从左侧表格中选择一名学生。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string studentNo = tbxSID.Text;
            string newIntro = ttbxIntro.Text;

            // **[重要]** 使用参数化查询防止SQL注入
            string sql = "UPDATE tblTopStudents SET Intro = @Intro WHERE studentNo = @studentNo";

            SqlParameter[] prams = {
                new SqlParameter("@Intro", SqlDbType.NVarChar, 50) { Value = newIntro },
                new SqlParameter("@studentNo", SqlDbType.NVarChar, 50) { Value = studentNo }
            };

            try
            {
                int rowsAffected = sh.RunSQL(sql, prams);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("更新成功！");
                    // **[优化]** 直接更新DataGridView中的数据，避免重新查询数据库，提高响应速度
                    dataGridView1.CurrentRow.Cells["Intro"].Value = newIntro;
                }
                else
                {
                    MessageBox.Show("更新失败，未找到该学生记录或数据无变化。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新过程中发生错误: " + ex.Message, "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sh.Close();
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            // 1. 获取数据 (这部分代码保持不变)
            string sql = "SELECT Major, COUNT(*) as StudentCount FROM tblTopStudents GROUP BY Major";
            DataSet ds = new DataSet();
            var majors = new List<string>();
            var counts = new List<double>();

            try
            {
                SQLHelper sh = new SQLHelper();
                sh.RunSQL(sql, ref ds);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("没有查询到可用于生成图表的数据。");
                    return;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    majors.Add(dr["Major"].ToString());
                    counts.Add(Convert.ToDouble(dr["StudentCount"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据失败: " + ex.Message);
                return;
            }

            // --- 以下是基于您提供的 ScottPlot 5 Cookbook 文档的正确用法 ---

            formsPlot1.Plot.Clear();

            // 步骤1: 使用数值数组创建饼图。
            // Add.Pie() 方法会自动在内部创建一组 PieSlice 对象。
            var pie = formsPlot1.Plot.Add.Pie(counts.ToArray());

            // 步骤2: 遍历自动生成的 Slices 集合，对每个扇区进行单独定制。
            // 这是设置不同标签和图例文字的正确方式。
            double totalCount = counts.Sum();
            for (int i = 0; i < pie.Slices.Count; i++)
            {
                var slice = pie.Slices[i];
                var majorName = majors[i];

                // 设置显示在“图例(Legend)”中的文字。
                slice.LegendText = majorName; // 例如: "计算机科学"

                // 设置显示在“扇区上”的标签文字。
                // 这里我们将其设置为百分比。
                slice.Label = $"{slice.Value / totalCount:P1}"; // P1 格式化为带一位小数的百分比, e.g., "45.5%"
            }
            string chineseFontName = "MicrosoftYaHei";

            // 2. 将图例的字体设置为您选择的中文字体。
            formsPlot1.Plot.Font.Automatic();
            // -----------------------------------------------------------------

            // (可选) 如果您的图表标题或其他地方也需要显示中文，可以一并设置
            //formsPlot1.Plot.TitleStyle.Font.Name = chineseFontName;


            //formsPlot1.Plot.Title("各专业学生人数分布"); // 确保这行在设置字体之后
            // 步骤3: 调用 ShowLegend() 来显示图例。
            formsPlot1.Plot.ShowLegend();

            // (可选) 调整扇区上标签的位置，0.5 表示在中间。
            //pie.SliceLabelPosition = 0.5;

            // --- 修改结束 ---

            formsPlot1.Refresh();
        }
    }
}
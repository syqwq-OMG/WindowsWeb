using GenerativeAI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NameRecognizerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void recognizeButton_Click(object sender, EventArgs e)
        {
            // --- 您的 API 密钥 ---
            string apiKey = "AIzaSyCOLYpUE-E8eUOo127GT-guepW-zxF_WjQ";
            // ---

            string inputText = inputTextBox.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("请输入要分析的文本。", "提示"); // <-- 修改为中文
                return;
            }
            if (apiKey == "在此处粘贴您的API密钥")
            {
                MessageBox.Show("请在代码中设置您的API密钥。", "错误"); // <-- 修改为中文
                return;
            }

            recognizeButton.Enabled = false;
            recognizeButton.Text = "正在识别中..."; // <-- 修改为中文
            resultTextBox.Text = "正在请求AI进行识别..."; // <-- 修改为中文

            try
            {
                var googleAI = new GoogleAi(apiKey);
                var model = googleAI.CreateGenerativeModel("models/gemini-1.5-flash");
                string prompt = $"任务：请从下面的文本中严格地提取出所有的人名。要求：1. 只返回人名，不要添加任何额外的解释或描述。2. 如果有多个人名，请使用中文逗号“，”进行分隔。3. 如果文本中没有找到任何人名，请准确地返回“未识别到人名”。\n\n待分析的文本如下：\n---\n{inputText}\n---\n\n提取的人名：";

                var response = await model.GenerateContentAsync(prompt);
                resultTextBox.Text = response.Text();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API错误: {ex.Message}", "错误"); // <-- 修改为中文
                resultTextBox.Text = "未能获取到响应。"; // <-- 修改为中文
            }
            finally
            {
                recognizeButton.Enabled = true;
                recognizeButton.Text = "开始识别"; // <-- 修改为中文
            }
        }
    }
}
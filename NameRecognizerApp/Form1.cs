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
            // --- ���� API ��Կ ---
            string apiKey = "AIzaSyCOLYpUE-E8eUOo127GT-guepW-zxF_WjQ";
            // ---

            string inputText = inputTextBox.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("������Ҫ�������ı���", "��ʾ"); // <-- �޸�Ϊ����
                return;
            }
            if (apiKey == "�ڴ˴�ճ������API��Կ")
            {
                MessageBox.Show("���ڴ�������������API��Կ��", "����"); // <-- �޸�Ϊ����
                return;
            }

            recognizeButton.Enabled = false;
            recognizeButton.Text = "����ʶ����..."; // <-- �޸�Ϊ����
            resultTextBox.Text = "��������AI����ʶ��..."; // <-- �޸�Ϊ����

            try
            {
                var googleAI = new GoogleAi(apiKey);
                var model = googleAI.CreateGenerativeModel("models/gemini-1.5-flash");
                string prompt = $"�������������ı����ϸ����ȡ�����е�������Ҫ��1. ֻ������������Ҫ����κζ���Ľ��ͻ�������2. ����ж����������ʹ�����Ķ��š��������зָ���3. ����ı���û���ҵ��κ���������׼ȷ�ط��ء�δʶ����������\n\n���������ı����£�\n---\n{inputText}\n---\n\n��ȡ��������";

                var response = await model.GenerateContentAsync(prompt);
                resultTextBox.Text = response.Text();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API����: {ex.Message}", "����"); // <-- �޸�Ϊ����
                resultTextBox.Text = "δ�ܻ�ȡ����Ӧ��"; // <-- �޸�Ϊ����
            }
            finally
            {
                recognizeButton.Enabled = true;
                recognizeButton.Text = "��ʼʶ��"; // <-- �޸�Ϊ����
            }
        }
    }
}
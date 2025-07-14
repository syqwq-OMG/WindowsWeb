// GeminiAiService.cs
using GenerativeAI;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace VoaDownloaderWpf
{
    public class GeminiAiService
    {
        private readonly GenerativeModel _model;
        private ChatSession _chatSession;

        public GeminiAiService(string apiKey)
        {
            // 1. 初始化模型，选择最新的flash模型，性价比高
            _model = new GenerativeModel(apiKey, model: "gemini-1.5-flash-latest");
        }

        /// <summary>
        /// 使用文章上下文开始一个新的聊天会话
        /// </summary>
        /// <param name="context">从文章中选中的文本</param>
        public void StartChatSession(string context)
        {
            // 2. 使用StartChat()开启一个可以保持上下文的聊天会话
            _chatSession = _model.StartChat();

            // 3. （可选但推荐）如果存在上下文，先将其作为背景信息发送给AI
            if (!string.IsNullOrWhiteSpace(context))
            {
                string initialPrompt = $"Please act as a helpful English learning assistant for a Chinese student. The user is currently reading an article. Here is a piece of text from the article that they have selected as context: \"{context}\". Now, please answer the user's following questions based on this context in Chinese. 接下来的所有内容请用中文回答。";

                // 发送初始上下文，但不等待回复，让其成为对话背景
                _ = _chatSession.GenerateContentAsync(initialPrompt);
            }
        }

        /// <summary>
        /// 发送一条消息并获取回复
        /// </summary>
        public async Task<string> SendMessageAsync(string userPrompt)
        {
            if (_chatSession == null)
            {
                // 如果没有上下文，直接开始一个空会话
                StartChatSession(string.Empty);
            }

            try
            {
                // 4. 发送用户的消息，AI会自动关联之前的对话历史
                var response = await _chatSession.GenerateContentAsync(userPrompt);

                // 5. 直接从response.Text获取纯文本回复，非常方便
                return response.Text();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AI服务出错: {ex.Message}", "API错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return "抱歉，与AI的通信出现问题。";
            }
        }
    }
}
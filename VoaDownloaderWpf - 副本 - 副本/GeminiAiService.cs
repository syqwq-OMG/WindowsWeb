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
            // 使用StartChat()开启一个可以保持上下文的聊天会话
            _chatSession = _model.StartChat();

            // =============================================================
            // ====          【核心修正】修改这里的系统提示词             ====
            // =============================================================

            // 1. 定义中文的系统提示词，明确要求AI的角色和回答语言
            string systemInstruction = "你是一个乐于助人的英语学习助手。请务必始终使用【中文】来回答我的所有问题。";

            // 2. 如果用户选择了上下文，将上下文信息附加到提示词中
            if (!string.IsNullOrWhiteSpace(context))
            {
                systemInstruction += $" 用户当前正在阅读一篇文章，这是他们选中的上下文内容，请你参考： \"{context}\"";
            }

            // 3. 将这个包含角色设定的完整提示词，作为对话的开场白发送给AI
            // 我们不需要等待它的回复，这更像是一个“背景设定”
            _ = _chatSession.GenerateContentAsync(systemInstruction);
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
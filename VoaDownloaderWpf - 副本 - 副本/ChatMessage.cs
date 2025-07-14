// ChatMessage.cs
namespace VoaDownloaderWpf
{
    public class ChatMessage
    {
        public string Author { get; set; } // "You" 或 "AI"
        public string Content { get; set; }
        public bool IsUserMessage { get; set; } // 用于UI判断消息来源
    }
}
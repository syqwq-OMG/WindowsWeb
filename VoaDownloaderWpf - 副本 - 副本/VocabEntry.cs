// VocabEntry.cs
using System;
using System.ComponentModel.DataAnnotations; // 需要此 using

namespace VoaDownloaderWpf
{
    public class VocabEntry
    {
        [Key] // 标记Id为主键
        public int Id { get; set; }

        [Required] // 标记为必需字段
        public string Word { get; set; }

        public string Definition { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? LastReviewDate { get; set; } // 上次复习时间，可以为空

        public int ReviewCount { get; set; }

        public bool IsLearned { get; set; }

        public bool IsDeleted { get; set; } // 软删除标记

        // [NotMapped] 特性告诉EF Core不要将这个属性保存到数据库
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsSelected { get; set; }

        // 【新增】用于存储单词上下文的句子
        //public string ContextSentence { get; set; }
    }
}
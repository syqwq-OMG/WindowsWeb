// PhraseEntry.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoaDownloaderWpf
{
    public class PhraseEntry:BaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } // 收藏的好词好句内容

        public string Notes { get; set; } // 用户可能添加的笔记或心得

        public string SourceArticleTitle { get; set; } // 来源文章标题

        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }

        private bool _isSelected;
        [NotMapped]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(); // 当IsSelected改变时，通知UI
            }
        }
    }
}
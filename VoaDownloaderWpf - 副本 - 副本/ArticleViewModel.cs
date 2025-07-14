﻿namespace VoaDownloaderWpf
{
    public class ArticleViewModel : BaseViewModel
    {
        private bool _isSelected;

        public string Title { get; set; }
        public string Url { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
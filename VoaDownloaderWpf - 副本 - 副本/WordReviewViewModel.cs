// WordReviewViewModel.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace VoaDownloaderWpf
{
    public class WordReviewViewModel : BaseViewModel
    {
        private readonly List<VocabEntry> _reviewList;
        private int _currentIndex = -1;

        public VocabEntry CurrentWord { get; private set; }
        public string ProgressText { get; private set; }
        public bool IsReviewFinished { get; private set; }

        // =============================================================
        // ====         【核心修正】改造IsDefinitionVisible属性        ====
        // =============================================================
        private bool _isDefinitionVisible;
        public bool IsDefinitionVisible
        {
            get => _isDefinitionVisible;
            private set
            {
                // 只有当值确实发生改变时才执行
                if (_isDefinitionVisible == value) return;
                _isDefinitionVisible = value;
                OnPropertyChanged(); // 在值改变后，立刻“通知”UI刷新 
            }
        }

        public ICommand ShowDefinitionCommand { get; }
        public ICommand MarkAsKnownCommand { get; }
        public ICommand MarkAsUnknownCommand { get; }
        public ICommand GoBackCommand { get; }

        public WordReviewViewModel(List<VocabEntry> wordsToReview)
        {
            // 命令的CanExecute会自动依赖IsDefinitionVisible的更改通知
            ShowDefinitionCommand = new RelayCommand(
                execute: _ => IsDefinitionVisible = true,
                canExecute: _ => !IsDefinitionVisible
            );

            MarkAsKnownCommand = new RelayCommand(_ => MarkWordAndMoveNext(true));
            MarkAsUnknownCommand = new RelayCommand(_ => MarkWordAndMoveNext(false));
            GoBackCommand = new RelayCommand(_ => GoToPreviousWord(), _ => _currentIndex > 0 && !IsReviewFinished);

            var random = new Random();
            _reviewList = wordsToReview.OrderBy(w => random.Next()).ToList();

            DisplayWordAtIndex(0);
        }

        private void DisplayWordAtIndex(int index)
        {
            if (index >= 0 && index < _reviewList.Count)
            {
                _currentIndex = index;
                CurrentWord = _reviewList[_currentIndex];
                IsDefinitionVisible = false; // 切换单词时，重置为不显示释义
                ProgressText = $"{_currentIndex + 1} / {_reviewList.Count}";
                IsReviewFinished = false;

                OnPropertyChanged(nameof(CurrentWord));
                OnPropertyChanged(nameof(ProgressText));
                OnPropertyChanged(nameof(IsReviewFinished));
            }
            else
            {
                IsReviewFinished = true;
                OnPropertyChanged(nameof(IsReviewFinished));
            }
        }

        private void MarkWordAndMoveNext(bool known)
        {
            if (CurrentWord == null) return;

            CurrentWord.ReviewCount++;
            CurrentWord.LastReviewDate = DateTime.Now;
            if (known)
            {
                CurrentWord.IsLearned = true;
            }

            VocabDataService.UpdateWordsStatus(new List<VocabEntry> { CurrentWord }, CurrentWord.IsLearned);

            DisplayWordAtIndex(_currentIndex + 1);
        }

        private void GoToPreviousWord()
        {
            if (_currentIndex > 0)
            {
                DisplayWordAtIndex(_currentIndex - 1);
            }
        }
    }
}
// VocabBookViewModel.cs
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace VoaDownloaderWpf
{
    public class VocabBookViewModel : BaseViewModel
    {
        // 【核心修正1】移除这个私有的、会变陈旧的本地缓存
        private List<VocabEntry> _allVocabEntries;

        public ObservableCollection<VocabEntry> VocabEntries { get; }
        // 【新增】用于绑定到UI的、待复习单词数量
        public int WordsToReviewCount { get; private set; }
        // 【新增】用于自由复习的单词数量
        public int AllUnlearnedWordsCount { get; private set; }

        // 【修改】将一个复习命令改为两个
        public ICommand EbbinghausReviewCommand { get; }
        public ICommand FreeReviewCommand { get; }


        public ICommand MarkSelectedAsLearnedCommand { get; }
        public ICommand MarkSelectedAsForgottenCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand ReviewWordsCommand { get; } // 【新增】复习单词命令

        private bool _showLearned;
        public bool ShowLearned
        {
            get => _showLearned;
            set
            {
                if (_showLearned == value) return;
                _showLearned = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        private bool? _isAllSelected;
        public bool? IsAllSelected
        {
            get => _isAllSelected;
            set
            {
                // 【修正1】检查值是否真的改变，防止无限循环
                if (_isAllSelected == value) return;

                _isAllSelected = value;

                // 【修正2】只有当IsAllSelected变为true或false时（即用户点击了全选框），才执行全选/全不选操作。
                // 如果是中间状态(null)，则不执行任何操作。
                if (_isAllSelected.HasValue)
                {
                    SelectAll(_isAllSelected.Value);
                }

                OnPropertyChanged();
            }
        }

        public VocabBookViewModel()
        {
            VocabEntries = new ObservableCollection<VocabEntry>();

            // 【核心修正2】构造函数不再需要加载数据到本地缓存
            // _allVocabEntries = VocabDataService.GetVocabList();

            MarkSelectedAsLearnedCommand = new RelayCommand(_ => MarkSelected(true), _ => VocabEntries.Any(e => e.IsSelected));
            MarkSelectedAsForgottenCommand = new RelayCommand(_ => MarkSelected(false), _ => VocabEntries.Any(e => e.IsSelected));
            DeleteSelectedCommand = new RelayCommand(_ => DeleteSelected(), _ => VocabEntries.Any(e => e.IsSelected));
            SelectionChangedCommand = new RelayCommand(_ => UpdateSelectAllState());


            // 【修改】初始化两个独立的复习命令
            EbbinghausReviewCommand = new RelayCommand(
                _ => OpenReviewWindow(VocabDataService.GetWordsForReview()),
                _ => WordsToReviewCount > 0
            );
            FreeReviewCommand = new RelayCommand(
                _ => OpenReviewWindow(VocabDataService.GetAllUnlearnedWords()),
                _ => AllUnlearnedWordsCount > 0
            );
            // 首次加载时应用一次筛选
            ApplyFilter();
        }
        // 【新增】打开背单词窗口的方法
        // 【修改】OpenReviewWindow 现在接收一个列表作为参数
        private void OpenReviewWindow(List<VocabEntry> wordsToReview)
        {
            var reviewViewModel = new WordReviewViewModel(wordsToReview);
            var reviewWindow = new WordReviewWindow
            {
                DataContext = reviewViewModel
            };
            reviewWindow.ShowDialog();

            _allVocabEntries = VocabDataService.GetVocabList();
            ApplyFilter();
        }
        private void MarkSelected(bool asLearned)
        {
            var selectedEntries = VocabEntries.Where(e => e.IsSelected).ToList();
            if (!selectedEntries.Any()) return;

            VocabDataService.UpdateWordsStatus(selectedEntries, asLearned);
            ApplyFilter();
        }

        private void DeleteSelected()
        {
            var selectedEntries = VocabEntries.Where(e => e.IsSelected).ToList();
            if (!selectedEntries.Any()) return;

            if (MessageBox.Show($"确定要从生词本中将这 {selectedEntries.Count} 个单词移入回收站吗？", "确认操作", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                VocabDataService.SoftDeleteWords(selectedEntries);
                ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            var allEntries = VocabDataService.GetVocabList();

            // 【优化】取消选中操作可以放到循环里，或者在SelectAll(false)里统一处理。
            // 这里我们先清除旧数据，再加载新数据，所以之前的选中状态自然会消失。

            VocabEntries.Clear();
            var filtered = _showLearned
                ? allEntries
                : allEntries.Where(e => !e.IsLearned);

            foreach (var entry in filtered)
            {
                VocabEntries.Add(entry);
            }

            // 加载完数据后，更新一次“全选”复选框的状态
            UpdateSelectAllState();
            UpdateReviewCount(); // 【新增】每次筛选后，都重新计算待复习数量
        }

        private void SelectAll(bool isSelected)
        {
            foreach (var entry in VocabEntries)
            {
                entry.IsSelected = isSelected;
            }
            // 因为VocabEntry没有实现INotifyPropertyChanged，需要手动刷新UI
            RefreshObservableCollection();
        }

        /// <summary>
        /// 【新增】一个专门的方法，用于更新待复习单词的数量
        /// </summary>
        private void UpdateReviewCount()
        {
            WordsToReviewCount = VocabDataService.GetWordsForReview().Count;
            AllUnlearnedWordsCount = VocabDataService.GetAllUnlearnedWords().Count; // 【新增】
            OnPropertyChanged(nameof(WordsToReviewCount));
            OnPropertyChanged(nameof(AllUnlearnedWordsCount)); // 【新增】
        }
        private void UpdateSelectAllState()
        {
            // 这个方法现在由单个CheckBox的Command触发
            if (!VocabEntries.Any())
            {
                // 【修正3】直接设置私有字段_isAllSelected并调用OnPropertyChanged，以避免触发setter中的SelectAll逻辑。
                // 这样做更安全，可以防止不必要的循环更新。
                _isAllSelected = false;
            }
            else if (VocabEntries.All(e => e.IsSelected))
            {
                _isAllSelected = true;
            }
            else if (VocabEntries.Any(e => e.IsSelected))
            {
                _isAllSelected = null; // 中间状态
            }
            else
            {
                _isAllSelected = false;
            }

            // 手动通知UI更新IsAllSelected属性
            OnPropertyChanged(nameof(IsAllSelected));
        }

        private void RefreshObservableCollection()
        {
            var items = VocabEntries.ToList();
            VocabEntries.Clear();
            foreach (var item in items)
            {
                VocabEntries.Add(item);
            }
            UpdateSelectAllState();
        }
    }
}
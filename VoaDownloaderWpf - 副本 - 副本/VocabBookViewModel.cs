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
        private readonly List<VocabEntry> _allVocabEntries;
        public ObservableCollection<VocabEntry> VocabEntries { get; }

        public ICommand MarkSelectedAsLearnedCommand { get; }
        public ICommand MarkSelectedAsForgottenCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
        public ICommand SelectionChangedCommand { get; }

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

        // 【核心】用于绑定“全选”复选框的属性
        private bool? _isAllSelected;
        public bool? IsAllSelected
        {
            get => _isAllSelected;
            set
            {
                // 只有当值发生变化时才执行，防止无限循环
                if (_isAllSelected == value) return;

                _isAllSelected = value;

                // 如果IsAllSelected有确定的值(true/false)，则执行全选/全不选
                if (_isAllSelected.HasValue)
                {
                    SelectAll(_isAllSelected.Value);
                }
                OnPropertyChanged();
            }
        }

        public VocabBookViewModel()
        {
            _allVocabEntries = VocabDataService.GetVocabList();
            VocabEntries = new ObservableCollection<VocabEntry>();

            MarkSelectedAsLearnedCommand = new RelayCommand(_ => MarkSelected(true), _ => VocabEntries.Any(e => e.IsSelected));
            MarkSelectedAsForgottenCommand = new RelayCommand(_ => MarkSelected(false), _ => VocabEntries.Any(e => e.IsSelected));
            DeleteSelectedCommand = new RelayCommand(_ => DeleteSelected(), _ => VocabEntries.Any(e => e.IsSelected));

            // 【核心】当单个复选框被点击时，触发此命令来更新“全选”复选框的状态
            SelectionChangedCommand = new RelayCommand(_ => UpdateSelectAllState());

            ApplyFilter();
        }

        private void MarkSelected(bool asLearned)
        {
            var selectedEntries = _allVocabEntries.Where(e => e.IsSelected).ToList();
            if (!selectedEntries.Any()) return;

            foreach (var entry in selectedEntries)
            {
                entry.IsLearned = asLearned;
                entry.IsSelected = false;
            }
            VocabDataService.SaveVocabBookAsync();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            // 在过滤前，先解除所有条目的选中状态
            foreach (var entry in _allVocabEntries)
            {
                entry.IsSelected = false;
            }

            VocabEntries.Clear();
            var filtered = _showLearned
                ? _allVocabEntries
                : _allVocabEntries.Where(e => !e.IsLearned);

            foreach (var entry in filtered)
            {
                VocabEntries.Add(entry);
            }
            UpdateSelectAllState(); // 每次过滤后都要更新“全选”状态
        }

        private void DeleteSelected()
        {
            var selectedEntries = _allVocabEntries.Where(e => e.IsSelected).ToList();
            if (!selectedEntries.Any()) return;

            if (MessageBox.Show($"确定要从生词本中彻底删除这 {selectedEntries.Count} 个单词吗？\n此操作不可恢复。", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                VocabDataService.RemoveWords(selectedEntries);
                foreach (var entry in selectedEntries)
                {
                    _allVocabEntries.Remove(entry);
                }
                ApplyFilter();
            }
        }

        /// <summary>
        /// 执行全选或全不选
        /// </summary>
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
        /// 根据当前列表中各项的选中状态，更新“全选”复选框的状态
        /// </summary>
        private void UpdateSelectAllState()
        {
            if (VocabEntries.All(e => e.IsSelected))
            {
                // 如果列表为空，IsAllSelected应为false
                IsAllSelected = VocabEntries.Any();
            }
            else if (VocabEntries.Any(e => e.IsSelected))
            {
                IsAllSelected = null; // 部分选中，显示为中间状态
            }
            else
            {
                IsAllSelected = false; // 全不选
            }
        }

        // 辅助方法，用于在全选后强制刷新UI
        private void RefreshObservableCollection()
        {
            var items = VocabEntries.ToList();
            VocabEntries.Clear();
            foreach (var item in items)
            {
                VocabEntries.Add(item);
            }
            // 刷新后，重新计算并更新“全选”复选框的状态
            UpdateSelectAllState();
        }
    }
}
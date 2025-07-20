// PhraseBookViewModel.cs
using Microsoft.Win32;
using System.IO; // 需要此 using
using System.Text; // 需要此 using
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace VoaDownloaderWpf
{
    public class PhraseBookViewModel : BaseViewModel
    {
        public ObservableCollection<PhraseEntry> PhraseEntries { get; }

        public ICommand DeleteSelectedCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand ExportCommand { get; } // 【新增】导出命令

        private bool? _isAllSelected;
        public bool? IsAllSelected
        {
            get => _isAllSelected;
            set
            {
                if (_isAllSelected == value) return;
                _isAllSelected = value;

                if (value.HasValue)
                {
                    SelectAll(value.Value);
                }

                OnPropertyChanged();
            }
        }

        public PhraseBookViewModel()
        {
            // 将 GetPhraseList() 修正为正确的方法名 GetAllPhrases()
            var entries = PhraseDataService.GetAllPhrases(); // <-- 正确的名称
            PhraseEntries = new ObservableCollection<PhraseEntry>(entries);

            DeleteSelectedCommand = new RelayCommand(
                _ => DeleteSelected(),
                _ => PhraseEntries.Any(p => p.IsSelected)
            );
            // 【新增】初始化新命令，只有当列表不为空时才可用
            ExportCommand = new RelayCommand(
                _ => ExportPhrases(),
                _ => PhraseEntries.Any()
            );
            SelectionChangedCommand = new RelayCommand(_ => UpdateSelectAllState());

            UpdateSelectAllState();
        }
        // 【新增】导出积累本到文件的方法
        // PhraseBookViewModel.cs

        private void ExportPhrases()
        {
            var dialog = new SaveFileDialog
            {
                Title = "导出积累本为Markdown",
                // 【已修正】默认文件名和文件类型
                Filter = "Markdown 文件 (*.md)|*.md|所有文件 (*.*)|*.*",
                FileName = "我的积累本.md"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("# 我的积累本"); // 为Markdown文件添加一个主标题
                    stringBuilder.AppendLine();

                    foreach (var entry in PhraseEntries)
                    {
                        // 【已修正】为原文（Content）添加Markdown的加粗语法
                        stringBuilder.AppendLine($"- **{entry.Content}** --{entry.SourceArticleTitle}");
                        stringBuilder.AppendLine(); // 添加一个空行以分隔条目
                    }

                    File.WriteAllText(dialog.FileName, stringBuilder.ToString());
                    MessageBox.Show("积累本已成功导出为Markdown文件！", "导出完成", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导出时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadData()
        {
            PhraseEntries.Clear();
            // This assumes PhraseDataService.GetPhraseList() exists.
            // Adapt this line to how you actually retrieve your phrase list.
            var phrases = PhraseDataService.GetAllPhrases();
            foreach (var phrase in phrases)
            {
                PhraseEntries.Add(phrase);
            }
            UpdateSelectAllState();
        }

        private void DeleteSelected()
        {
            var selected = PhraseEntries.Where(p => p.IsSelected).ToList();
            if (MessageBox.Show($"确定要删除这 {selected.Count} 个条目吗?", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                PhraseDataService.DeletePhrases(selected);
                LoadData(); // Reload the data from the source to reflect the deletion.
            }
        }

        private bool CanDeleteSelected()
        {
            return PhraseEntries.Any(p => p.IsSelected);
        }

        private void SelectAll(bool select)
        {
            foreach (var entry in PhraseEntries)
            {
                entry.IsSelected = select;
            }
            // IMPORTANT: If PhraseEntry does not implement INotifyPropertyChanged,
            // the UI won't update automatically. You would need a refresh mechanism here.
            // However, let's proceed assuming the check/uncheck will trigger UI updates via the command.
            UpdateSelectAllState(); // Ensure the command predicate is re-evaluated
        }

        private void UpdateSelectAllState()
        {
            if (!PhraseEntries.Any())
            {
                _isAllSelected = false;
            }
            else if (PhraseEntries.All(p => p.IsSelected))
            {
                _isAllSelected = true;
            }
            else if (PhraseEntries.Any(p => p.IsSelected))
            {
                _isAllSelected = null;
            }
            else
            {
                _isAllSelected = false;
            }
            OnPropertyChanged(nameof(IsAllSelected));

            // This forces the CanExecute on commands to be re-evaluated
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VoaDownloaderWpf
{
    public static class VocabDataService
    {
        private static List<VocabEntry> _vocabList;
        private static readonly string _filePath;

        // 静态构造函数：在程序生命周期中只执行一次，确保路径和列表的初始化
        static VocabDataService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "VoaDownloaderWpf");
            Directory.CreateDirectory(appFolder);
            _filePath = Path.Combine(appFolder, "vocab.json");
            _vocabList = new List<VocabEntry>(); // 初始化为空列表
        }

        // 从JSON文件加载生词本到内存 (最关键的修复)
        public static void LoadVocabBook()
        {
            if (!File.Exists(_filePath))
            {
                // 如果文件不存在，确保列表是空的，然后直接返回
                _vocabList.Clear();
                return;
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                // 【核心修正】: 使用赋值操作(=)来完全替换内存中的列表。
                // 这确保了无论此方法被调用多少次，内存中的数据都只等于文件中的数据，永远不会重复累加。
                _vocabList = JsonConvert.DeserializeObject<List<VocabEntry>>(json) ?? new List<VocabEntry>();
            }
            catch (Exception)
            {
                // 如果文件损坏，则清空列表，保证程序不会因错误数据崩溃
                _vocabList.Clear();
            }
        }

        // 保存方法 (保持不变)
        public static async Task SaveVocabBookAsync()
        {
            string json = JsonConvert.SerializeObject(_vocabList, Formatting.Indented);
            await Task.Run(() => File.WriteAllText(_filePath, json));
        }

        // 添加或更新方法 (保持不变)
        public static void AddOrUpdateWord(string word, string definition)
        {
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(definition)) return;
            var existingEntry = _vocabList.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
            if (existingEntry != null)
            {
                existingEntry.LookupCount++;
            }
            else
            {
                var newEntry = new VocabEntry
                {
                    Word = word,
                    Definition = definition,
                    DateAdded = DateTime.Now,
                    LookupCount = 1,
                    IsLearned = false // 【新增】确保新词默认为“未学会”
                };
                _vocabList.Add(newEntry);
            }
            _ = SaveVocabBookAsync();
        }

        // 【新增】从生词本中彻底删除多个单词
        public static void RemoveWords(List<VocabEntry> wordsToRemove)
        {
            if (wordsToRemove == null || !wordsToRemove.Any()) return;

            // 为了高效删除，将要删除的单词存入HashSet
            var wordsToRemoveSet = new HashSet<string>(wordsToRemove.Select(w => w.Word));

            _vocabList.RemoveAll(entry => wordsToRemoveSet.Contains(entry.Word));

            // 保存更改
            _ = SaveVocabBookAsync();
        }

        // 获取列表方法 (保持不变)
        public static List<VocabEntry> GetVocabList()
        {
            return new List<VocabEntry>(_vocabList);
        }
    }
}
// VocabDataService.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoaDownloaderWpf
{
    public static class VocabDataService
    {
        private static List<VocabEntry> _vocabList; // 继续使用内存列表作为缓存

        // 应用启动时，从数据库加载数据到内存缓存
        public static void LoadVocabBookFromDb()
        {
            using (var db = new VocabDbContext())
            {
                // 只加载未被软删除的单词
                _vocabList = db.VocabEntries.Where(e => !e.IsDeleted).ToList();
            }
        }

        // 添加或更新一个单词
        public static void AddOrUpdateWord(string word, string definition)
        {
            using (var db = new VocabDbContext())
            {
                // 【核心修正】修正后的代码
                var existingEntry = db.VocabEntries.FirstOrDefault(e => e.Word.ToLower() == word.ToLower());
                if (existingEntry != null)
                {
                    // 如果存在，增加复习次数，更新复习时间
                    existingEntry.ReviewCount++;
                    existingEntry.LastReviewDate = DateTime.Now;
                    // 如果它之前被删除了，现在恢复它
                    existingEntry.IsDeleted = false;
                }
                else
                {
                    // 如果不存在，创建新条目
                    var newEntry = new VocabEntry
                    {
                        Word = word,
                        Definition = definition,
                        DateAdded = DateTime.Now,
                        LastReviewDate = DateTime.Now,
                        ReviewCount = 1,
                        IsLearned = false,
                        IsDeleted = false
                    };
                    db.VocabEntries.Add(newEntry);
                }
                db.SaveChanges(); // 保存所有更改到数据库
            }
            LoadVocabBookFromDb(); // 重新加载内存缓存
        }

        // 更新多个单词的状态（已学会/未学会）
        public static void UpdateWordsStatus(List<VocabEntry> wordsToUpdate, bool isLearned)
        {
            using (var db = new VocabDbContext())
            {
                var wordIds = wordsToUpdate.Select(w => w.Id).ToList();
                var entriesInDb = db.VocabEntries.Where(e => wordIds.Contains(e.Id)).ToList();

                foreach (var entry in entriesInDb)
                {
                    entry.IsLearned = isLearned;
                    entry.LastReviewDate = DateTime.Now; // 标记状态也算一次复习
                }
                db.SaveChanges();
            }
            LoadVocabBookFromDb();
        }

        // 软删除多个单词
        public static void SoftDeleteWords(List<VocabEntry> wordsToDelete)
        {
            using (var db = new VocabDbContext())
            {
                var wordIds = wordsToDelete.Select(w => w.Id).ToList();
                var entriesInDb = db.VocabEntries.Where(e => wordIds.Contains(e.Id)).ToList();

                foreach (var entry in entriesInDb)
                {
                    entry.IsDeleted = true;
                }
                db.SaveChanges();
            }
            LoadVocabBookFromDb();
        }

        // 获取列表方法 (保持不变，从内存缓存读取)
        public static List<VocabEntry> GetVocabList()
        {
            return _vocabList ?? new List<VocabEntry>();
        }

        /// <summary>
        /// 【核心新增】根据艾宾浩斯遗忘曲线获取今天需要复习的单词列表
        /// </summary>
        public static List<VocabEntry> GetWordsForReview()
        {
            // 艾宾浩斯曲线的典型复习周期（单位：天）
            // 第1天、第2天、第4天、第7天、第15天...
            int[] reviewIntervals = { 1, 2, 4, 7, 15, 30, 60, 90 };

            using (var db = new VocabDbContext())
            {
                var today = DateTime.Today;

                // 我们只筛选那些“未学会”且“未被删除”的单词
                return db.VocabEntries
                    .Where(e => !e.IsDeleted && !e.IsLearned)
                    .ToList() // 将数据加载到内存中进行日期计算
                    .Where(word => {
                        // 计算从添加日期到今天过去了多少天
                        var daysPassed = (today - word.DateAdded.Date).TotalDays;
                        // 如果今天正好是某个复习周期日，则该单词需要复习
                        return reviewIntervals.Contains((int)daysPassed);
                    })
                    .ToList();
            }
        }

        // 【新增】获取所有未学会的单词，用于自由复习
        public static List<VocabEntry> GetAllUnlearnedWords()
        {
            using (var db = new VocabDbContext())
            {
                return db.VocabEntries.Where(e => !e.IsDeleted && !e.IsLearned).ToList();
            }
        }
    }
}
// PhraseDataService.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace VoaDownloaderWpf
{
    public static class PhraseDataService
    {
        public static void AddPhrase(string content, string sourceArticle)
        {
            using (var db = new VocabDbContext())
            {
                var newPhrase = new PhraseEntry
                {
                    Content = content,
                    SourceArticleTitle = sourceArticle,
                    DateAdded = DateTime.Now,
                    IsDeleted = false,
                    // =============================================
                    // ====         【核心修正】添加此行           ====
                    // =============================================
                    Notes = string.Empty // 确保Notes字段永远不为null
                };
                db.PhraseEntries.Add(newPhrase);
                db.SaveChanges();
            }
        }

        public static List<PhraseEntry> GetAllPhrases()
        {
            using (var db = new VocabDbContext())
            {
                return db.PhraseEntries.Where(p => !p.IsDeleted).ToList();
            }
        }

        public static void DeletePhrases(List<PhraseEntry> phrasesToDelete)
        {
            using (var db = new VocabDbContext())
            {
                var phraseIds = phrasesToDelete.Select(p => p.Id).ToList();
                var entriesInDb = db.PhraseEntries.Where(p => phraseIds.Contains(p.Id)).ToList();

                foreach (var entry in entriesInDb)
                {
                    entry.IsDeleted = true;
                }
                db.SaveChanges();
            }
        }
    }
}
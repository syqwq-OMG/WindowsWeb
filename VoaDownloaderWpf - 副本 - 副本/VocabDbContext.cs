// VocabDbContext.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace VoaDownloaderWpf
{
    public class VocabDbContext : DbContext
    {
        // 这个属性代表了数据库中的一张表
        public DbSet<VocabEntry> VocabEntries { get; set; }

        private readonly string _dbPath;

        public VocabDbContext()
        {
            // AppContext.BaseDirectory 可以获取程序 .exe 文件所在的目录
            string baseDirectory = AppContext.BaseDirectory;

            // 将数据库文件命名为 vocab.db 并将其路径设置在程序根目录下
            _dbPath = Path.Combine(baseDirectory, "vocab.db");
        }

        // 配置DbContext使用SQLite数据库
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }
}
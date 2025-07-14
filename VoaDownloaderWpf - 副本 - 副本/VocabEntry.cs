// VocabEntry.cs
using System;
using Newtonsoft.Json;

namespace VoaDownloaderWpf
{
    public class VocabEntry
    {
        public string Word { get; set; }
        public string Definition { get; set; }
        public DateTime DateAdded { get; set; }
        public int LookupCount { get; set; }
        public bool IsLearned { get; set; } // 【新增】是否已学会的标记

        [JsonIgnore] // 此特性确保 IsSelected 属性不会被序列化到JSON文件中
        public bool IsSelected { get; set; }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VoaDownloaderWpf
{
    public static class DictionaryService
    {
        // 使用字典来存储单词和释义，提供O(1)级别的查找速度
        private static Dictionary<string, string> _dictionary;

        /// <summary>
        /// 异步加载词典文件到内存中
        /// </summary>
        /// <param name="filePath">ecdict.mini.csv 文件的路径</param>
        public static async Task LoadDictionaryAsync(string filePath)
        {
            if (_dictionary != null)
            {
                return;
            }

            _dictionary = new Dictionary<string, string>();
            var lines = await Task.Run(() => File.ReadAllLines(filePath));

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length > 3)
                {
                    string word = parts[0].Trim().ToLower();
                    string translation = parts[3].Trim();

                    //// 【新增修正】检查并移除字段两端的双引号
                    //if (translation.StartsWith("\"") && translation.EndsWith("\""))
                    //{
                    //    translation = translation.Substring(1, translation.Length - 1);
                    //}
                    if (translation.StartsWith("\""))
                    {
                        translation=translation.Substring(1, translation.Length -1);
                    }

                    if (!_dictionary.ContainsKey(word))
                    {
                        _dictionary.Add(word, translation);
                    }
                }
            }
        }

        /// <summary>
        /// 根据单词查找中文释义
        /// </summary>
        /// <param name="word">要查找的单词</param>
        /// <returns>找到的释义，或提示信息</returns>
        public static string GetDefinition(string word)
        {
            if (_dictionary == null || string.IsNullOrWhiteSpace(word))
            {
                return "词典未加载或单词为空。";
            }

            // 同样转为小写进行查找
            if (_dictionary.TryGetValue(word.Trim().ToLower(), out string definition))
            {
                // 格式化输出，将\n替换为真正的换行
                return definition.Replace("\\n", "\n");
            }
            else
            {
                return "字典未收录该单词。";
            }
        }
    }
}
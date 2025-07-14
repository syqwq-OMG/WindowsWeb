// App.xaml.cs
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace VoaDownloaderWpf
{
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // 设置配置文件所在的目录
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // 【新增】加载本地词典
            string dictionaryPath = Path.Combine(Directory.GetCurrentDirectory(), "ecdict.mini.csv");
            if (File.Exists(dictionaryPath))
            {
                await DictionaryService.LoadDictionaryAsync(dictionaryPath);
            }

            // 【新增】加载生词本数据
            VocabDataService.LoadVocabBook();
            base.OnStartup(e);
        }

    }
}
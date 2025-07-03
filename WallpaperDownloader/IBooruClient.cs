// IBooruClient.cs
using System.Collections.Generic;
using System.Threading.Tasks;
namespace WallpaperDownloader
{
    public interface IBooruClient
    {
        string SiteName { get; }
        Task<List<BooruImage>> GetImagesAsync(int page = 1, int limit = 40);
    }
}

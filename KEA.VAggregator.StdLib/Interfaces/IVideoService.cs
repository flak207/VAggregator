using KEA.VAggregator.StdLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KEA.VAggregator.StdLib.Services
{
    public interface IVideoService
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<IEnumerable<Video>> GetVideos(Category category = null);

        Task<IEnumerable<Video>> SearchVideos(string text, VideoQuality videoQuality = VideoQuality.Unknown, int count = 20);

        //string GetVideoPlayUrl(Video video);

        //Dictionary<string, string> GetVideoQualityUrls(Video video);

        Task FillVideoUrlsAndInfo(Video video);
    }
}

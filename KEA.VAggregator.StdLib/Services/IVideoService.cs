﻿using KEA.VAggregator.StdLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KEA.VAggregator.StdLib.Services
{
    public interface IVideoService
    {
        IEnumerable<Category> GetCategories();

        IEnumerable<Video> GetVideos(Category category = null);

        IEnumerable<Video> SearchVideos(string text, VideoQuality videoQuality = VideoQuality.Unknown);

        string GetVideoPlayUrl(Video video);

        Dictionary<string, string> GetVideoQualityUrls(Video video);

        void FillVideoUrlsAndInfo(Video video);
    }
}

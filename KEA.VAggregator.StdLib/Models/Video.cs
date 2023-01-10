using System;
using System.Collections.Generic;
using System.Text;

namespace KEA.VAggregator.StdLib.Models
{
    public class Video : WebItem
    {
        //public Uri SourceUri { get => PlayLink != null ? _sourceUri = new Uri(PlayLink) : null; }

        public string PlayLink { get; set; }

        public Dictionary<string, string> QualityLinks { get; set; } = new Dictionary<string, string>();

        public string Quality { get; set; }

        public string Duration { get; set; }

        public TimeSpan DurationTs { get; set; }

        public List<string> ScreenshotUrls { get; set; } = new List<string>();

        public string PreviewVideoUrl { get; set; } 

        public string Info { get; set; } = string.Empty;

        public Dictionary<string, string> InfoUrls { get; set; } = new Dictionary<string, string>();

        public IEnumerable<Video> RelatedVideos { get; set; } = new List<Video>();

        public int Page { get; set; } = 1;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace KEA.VAggregator.StdLib.Models
{
    public class Video : WebItem
    {
        private Uri _sourceUri = null;

        public Uri SourceUri { get => _sourceUri != null ? _sourceUri :(PlayLink != null ? _sourceUri = new Uri(PlayLink) : null); }

        public string PlayLink { get; set; }

        public Dictionary<string, string> QualityLinks { get; set; } = new Dictionary<string, string>();

        public string Quality { get; set; }

        public string Duration { get; set; }

        public TimeSpan DurationTs { get; set; }

        public List<string> ScreenshotUrls { get; set; } = new List<string>();

        public string Info { get; set; } = string.Empty;

        public Dictionary<string, string> InfoUrls { get; set; } = new Dictionary<string, string>();

        public IEnumerable<Video> RelatedVideos { get; set; } = new List<Video>();
    }
}

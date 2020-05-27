using System;
using System.Collections.Generic;
using System.Text;

namespace KEA.VAggregator.StdLib.Models
{
    public class Video : WebItem
    {
        public string PlayUrl { get; set; }

        public Dictionary<string, string> QualityUrls { get; set; } = new Dictionary<string, string>();

        public string Quality { get; set; }

        public string Duration { get; set; }

        public List<string> ScreenshotUrls { get; set; } = new List<string>();

        public string Info { get; set; } = string.Empty;
    }
}

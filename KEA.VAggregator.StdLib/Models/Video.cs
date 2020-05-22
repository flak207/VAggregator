using System;
using System.Collections.Generic;
using System.Text;

namespace KEA.VAggregator.StdLib.Models
{
    public class Video : WebItem
    {
        public string PlayUrl { get; set; }

        public Dictionary<string, string> QualityUrls { get; set; }
    }
}

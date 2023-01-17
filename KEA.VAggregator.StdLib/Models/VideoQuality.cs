using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace KEA.VAggregator.StdLib.Models
{
    public enum VideoQuality
    {
        [Description("Unknown")]
        Unknown = 0,
        //_144p = 144,
        //_240p = 240,
        //_360p = 360,
        [Description("480")]
        _480p = 480,
        [Description("720")]
        _720p = 720,
        [Description("1080")]
        _1080p = 1080,
        [Description("1440")]
        _1440p = 1440,
        [Description("2160")]
        _2160p = 2160
    }

    public static class EnumString
    {
        public static string GetDescription<T>(this T val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}

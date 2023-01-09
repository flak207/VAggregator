using KEA.VAggregator.StdLib.Models;
using KEA.VAggregator.StdLib.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace KEA.VAggregator.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly IVideoService _videoService = new TestVideoService();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var items = _videoService.GetVideos();
            LoadVideos(items);
        }

        public void LoadVideos(IEnumerable<Video> videos)
        {
            if (videos != null && wrapPanel != null)
            {
                // wrapPanel.ItemsSource = videos;
                if (videos != null)
                {
                    foreach (var video in videos)
                    {
                        var image = new Image
                        {
                            Source = ImageSource.FromUri(new Uri(video.ImageUrl))
                        };
                        wrapPanel.Children.Add(image);
                    }
                }
            }
        }


    }
}

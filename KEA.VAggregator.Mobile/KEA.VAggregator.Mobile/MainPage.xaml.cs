using KEA.VAggregator.StdLib.Models;
using KEA.VAggregator.StdLib.Services;
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
        private IVideoService _videoService => DependencyService.Get<IVideoService>(); 

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);

            var items = await _videoService.GetVideos();
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
                        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += async (s, e) =>
                        {
                            await _videoService.FillVideoUrlsAndInfo(video);
                            await Shell.Current.GoToAsync($"{nameof(VideoPage)}?{nameof(VideoPage.PlayLink)}={video.PlayLink}");
                        };
                        image.GestureRecognizers.Add(tapGestureRecognizer);
                        wrapPanel.Children.Add(image);
                    }
                }
            }
        }


    }
}

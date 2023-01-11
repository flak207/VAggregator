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

            var categories = await _videoService.GetCategories();
            categories = categories.OrderBy(c => c.Name);
            categoryPanel.ItemsSource = categories;

            var items = await _videoService.GetVideos();
            LoadVideos(items);
        }

        public void LoadVideos(IEnumerable<Video> videos)
        {
            if (wrapPanel != null)
            {
                wrapPanel.Children.Clear();
                // wrapPanel.ItemsSource = videos;
                if (videos != null)
                {
                    foreach (var video in videos)
                    {
                        var image = new Image
                        {
                            Source = ImageSource.FromUri(new Uri(video.ImageUrl))
                        };
                        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer() { NumberOfTapsRequired = 2 };
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

        private async void categoryChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = categoryPanel.SelectedItem as Category;
            if (category != null)
            {
                var items = await _videoService.GetVideos(category);
                LoadVideos(items);
            }
        }
    }
}

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
using System.Windows.Input;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace KEA.VAggregator.Mobile
{
    public partial class MainPage : ContentPage
    {
        private IVideoService _videoService => DependencyService.Get<IVideoService>();
        private int _page = 1;

        public ICommand LongPressCommand { get; }

        public MainPage()
        {
            InitializeComponent();

            LongPressCommand = CommandFactory.Create(async (object parameter) =>
            {
                Video video = parameter as Video;
                //DisplayAlert($"{video.Name} clicked", null, "OK");
                if (video != null)
                {
                    await _videoService.FillVideoUrlsAndInfo(video);
                    LoadVideos(video.RelatedVideos);
                }
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);
            if (categoryPanel.ItemsSource == null)
            {
                var categories = await _videoService.GetCategories();
                categories = categories.OrderBy(c => c.Name);
                categoryPanel.ItemsSource = categories;
            }
            if (wrapPanel.ItemsSource == null)
            {
                var items = await _videoService.GetVideos();
                LoadVideos(items);
            }
        }

        public void LoadVideos(IEnumerable<Video> videos)
        {
            if (wrapPanel != null && videos != null)
            {
                var list = videos.ToList(); // new List<Video>() { videos.FirstOrDefault() }; 
                wrapPanel.ItemsSource = list;
            }
        }

        private async void categoryChanged(object sender, SelectionChangedEventArgs e)
        {
            _page = 1;
            Category category = categoryPanel.SelectedItem as Category;
            if (category != null)
            {
                var items = await _videoService.GetVideos(category, _page);
                LoadVideos(items);
            }
        }

        private async void videoDoubleTapped(object sender, EventArgs e)
        {
            Video video = (sender as BindableObject).BindingContext as Video;
            if (video != null)
            {
                await _videoService.FillVideoUrlsAndInfo(video);
                await Shell.Current.GoToAsync($"{nameof(VideoPage)}?{nameof(VideoPage.PlayLink)}={video.PlayLink}");
            }
        }

        private async void searchButton_Clicked(object sender, EventArgs e)
        {
            string searchTxt = searchInput.Text;
            Category category = categoryPanel.SelectedItem as Category;
            var items = string.IsNullOrEmpty(searchTxt) ? await _videoService.GetVideos(category, ++_page)
                : await _videoService.SearchVideos(searchTxt, VideoQuality.Unknown, (50 * _page++));
            LoadVideos(items);
        }

        private void searchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _page = 1;
        }
    }
}

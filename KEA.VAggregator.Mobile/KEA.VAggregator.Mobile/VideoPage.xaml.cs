using KEA.VAggregator.StdLib.Models;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using static System.TimeZoneInfo;

namespace KEA.VAggregator.Mobile
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    //[QueryProperty(nameof(PlayLink), nameof(PlayLink))]
    public partial class VideoPage : ContentPage
    {
        private Video _video = null;
        private int _imageIndex = 0;
        private bool _timerAlive = false;
        //public string PlayLink { get; set; }

        public VideoPage(Video video)
        {
            InitializeComponent();
            _video = video;
            mediaElement.MediaOpened += MediaElement_MediaOpened;
            mediaElement.MediaFailed += MediaElement_MediaFailed;
            //ImageUrl = video.ImageUrl;
            //PlayLink = video.PlayLink;
        }

        private void MediaElement_MediaFailed(object sender, EventArgs e)
        {
            Shell.Current.SendBackButtonPressed();
            // await Shell.Current.GoToAsync(S)
        }

        private void MediaElement_MediaOpened(object sender, EventArgs e)
        {
            progressGrid.IsVisible = _timerAlive = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);
            if (_video != null)
            {
                progressImage.Source = _video.ImageUrl;
                mediaElement.Source = _video.PlayLink;
                if (_video.ScreenshotUrls.Count > 1)
                {
                    _timerAlive = true;
                    Device.StartTimer(TimeSpan.FromSeconds(2), OnTimerTick);
                }
            }
            await UpdateProgressBar(1.00, 14000);
        }

        private bool OnTimerTick()
        {
            try
            {
                if (_video.ScreenshotUrls.Count > _imageIndex)
                {
                    // progressImage.FadeTo(0.0, 500, Easing.Linear);
                    progressImage.Source = _video.ScreenshotUrls[_imageIndex];
                    // progressImage.FadeTo(1.0, 500, Easing.Linear);

                    int newIndex = _imageIndex + 1;
                    _imageIndex = newIndex >= _video.ScreenshotUrls.Count ? 0 : newIndex;
                }
            }
            catch (Exception)
            {
                _timerAlive = false;
            }
            return _timerAlive;
        }

        async Task UpdateProgressBar(double progress, uint time)
        {
            await progressBar.ProgressTo(progress, time, Easing.Linear);
        }
    }
}
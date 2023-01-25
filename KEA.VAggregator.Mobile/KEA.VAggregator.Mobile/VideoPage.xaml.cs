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
                    DisplayProgess();
                }
                await UpdateProgressBar(1.00, 20000);
            }
        }

        private async void DisplayProgess()
        {
            try
            {
                while (_timerAlive)
                {
                    uint transitionTime = 1000;
                    double displacement = progressImage.Width / 2;

                    await Task.WhenAll(
                       progressImage.FadeTo(0.2, transitionTime, Easing.Linear),
                       progressImage.TranslateTo(-displacement, progressImage.Y, transitionTime, Easing.CubicInOut));
                    // Changes image source.
                    progressImage.Source = _video.ScreenshotUrls[_imageIndex];
                    await progressImage.TranslateTo(displacement, 0, 0);
                    await Task.WhenAll(
                        progressImage.FadeTo(1, transitionTime, Easing.Linear),
                        progressImage.TranslateTo(0, progressImage.Y, transitionTime, Easing.CubicInOut));

                    //await progressImage.FadeTo(0.5, 1000, Easing.Linear);
                    //progressImage.Source = _video.ScreenshotUrls[_imageIndex];
                    //await progressImage.FadeTo(1.0, 1000, Easing.Linear);

                    int newIndex = _imageIndex + 1;
                    _imageIndex = newIndex >= _video.ScreenshotUrls.Count ? 0 : newIndex;
                    //await Task.Delay(1000);
                }
            }
            catch (Exception)
            {
                _timerAlive = false;
            }
        }

        async Task UpdateProgressBar(double progress, uint time)
        {
            await progressBar.ProgressTo(progress, time, Easing.Linear);
        }
    }
}
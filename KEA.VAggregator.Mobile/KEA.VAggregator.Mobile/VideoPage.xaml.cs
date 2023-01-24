using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;

namespace KEA.VAggregator.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(PlayLink), nameof(PlayLink))]
    public partial class VideoPage : ContentPage
    {
        private string _playLink = string.Empty;

        public string PlayLink
        {
            set => _playLink = value;
        }

        public VideoPage()
        {
            InitializeComponent();
            mediaElement.MediaOpened += MediaElement_MediaOpened;
            mediaElement.MediaFailed += MediaElement_MediaFailed;
        }

        private void MediaElement_MediaFailed(object sender, EventArgs e)
        {
            Shell.Current.SendBackButtonPressed();
            // await Shell.Current.GoToAsync(S)
        }

        private void MediaElement_MediaOpened(object sender, EventArgs e)
        {
            progressBar.IsVisible = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);

            if (!string.IsNullOrEmpty(_playLink))
            {
                mediaElement.Source = _playLink;
            }
            await UpdateProgressBar(1.00, 20000);
        }

        async Task UpdateProgressBar(double progress, uint time)
        {
            await progressBar.ProgressTo(progress, time, Easing.Linear);
        }
    }
}
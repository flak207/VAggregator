using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
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
            set
            {
                _playLink = value;
            }
        }

        public VideoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetTabBarIsVisible(this, false);
            Shell.SetNavBarIsVisible(this, false);

            if (!string.IsNullOrEmpty(_playLink))
            {
                mediaElement.Source = _playLink;
            }
        }
    }
}
using KEA.VAggregator.StdLib.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KEA.VAggregator.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<IVideoService, TestVideoService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

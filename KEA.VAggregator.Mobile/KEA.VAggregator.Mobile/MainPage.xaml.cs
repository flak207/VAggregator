﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace KEA.VAggregator.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnMediaOpened(object sender, EventArgs e) => Console.WriteLine("Media opened.");

        void OnMediaFailed(object sender, EventArgs e) => Console.WriteLine("Media failed.");

        void OnMediaEnded(object sender, EventArgs e) => Console.WriteLine("Media ended.");

        void OnSeekCompleted(object sender, EventArgs e) => Console.WriteLine("Seek completed.");

        void OnResetClicked(object sender, EventArgs e) => mediaElement.Source = null;


        private void Slider_DragCompleted(object sender, EventArgs e)
        {
            mediaElement.Speed = MainSlider.Value;
        }
    }
}

﻿using KEA.VAggregator.StdLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KEA.VAggregator.WPF
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        private Video _video = null;

        public InfoWindow(Video video)
        {
            _video = video;

            InitializeComponent();
            txtInfo.Text = video?.Info + video.Description;
        }

        private void btnRelatedVideos_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            mainWindow.LoadVideos(_video.RelatedVideos);
            mainWindow.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //base.OnClosing(e);
        }

    }
}

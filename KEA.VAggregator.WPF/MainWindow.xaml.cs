using KEA.VAggregator.StdLib.Models;
using KEA.VAggregator.StdLib.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace KEA.VAggregator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields

        private readonly IVideoService _videoService = new TestVideoService();
        private DispatcherTimer _screenshotTimer; // = new DispatcherTimer(); 
        private int _count = 20;

        

        public event PropertyChangedEventHandler PropertyChanged;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
        }
        #endregion


        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            _screenshotTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(700), DispatcherPriority.Normal, screenshotTimer_Tick, this.Dispatcher);
            Loaded += MainWindow_Loaded;
            this.DataContext = this;
        }
        #endregion

        #region Methods

        public void SearchVideos()
        {
            var items = _videoService.SearchVideos(searchInput.Text, (VideoQuality)cmbVideoQuality.SelectedItem, this.Count);
            LoadVideos(items);
        }

        public void LoadVideos(IEnumerable<Video> videos)
        {
            if (videos != null && wrapPanel != null)
                wrapPanel.ItemsSource = videos;
        }
        #endregion

        #region Event Handlers

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (wrapPanel.ItemsSource == null)
            {
                var items = _videoService.GetVideos(); //.GetCategories().OrderBy(c => c.Name);
                LoadVideos(items);

                var categories = _videoService.GetCategories();
                categories = categories.OrderBy(c => c.Name);
                categoriesList.ItemsSource = categories;
            }
        }

        private void wrapPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebItem selectedItem = wrapPanel.SelectedItem as WebItem;
            if (selectedItem != null)
                searchInput.Text = selectedItem.Name;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchVideos();
        }

        private void searchInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchVideos();
        }

        private void miCopyName_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Item item = menuItem?.DataContext as Item;
            if (item != null)
            {
                Clipboard.SetText(item.Name, TextDataFormat.Text);
            }
        }

        private void itemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WebItem selectedItem = wrapPanel.SelectedItem as WebItem;
            if (selectedItem as Video != null && e.ClickCount > 1)
            {
                Video video = selectedItem as Video;
                _videoService.FillVideoUrlsAndInfo(video);

                if (video.PlayLink != null)
                {
                    VideoWindow videoWindow = new VideoWindow(); // { Owner = this };
                    videoWindow.Show();
                    //video.PlayUrl = _videoService.GetVideoUrl(video);
                    videoWindow.PlayVideo(video);
                }
                else
                {
                    Process.Start(video.TargetUrl);
                }
            }
        }

        private void screenshotTimer_Tick(object sender, EventArgs e)
        {
            if (_screenshotTimer.Tag is Image image && image.DataContext is Video video
                && image.Tag is int index && video.ScreenshotUrls.Count > index)
            {
                image.Source = new BitmapImage(new Uri(video.ScreenshotUrls[index]));
                int newIndex = index + 1;
                image.Tag = newIndex >= video.ScreenshotUrls.Count ? 0 : newIndex;
            }
        }

        private void previewImage_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image image && image.DataContext is Video video && video.ScreenshotUrls.Count > 0)
            {
                image.Tag = image.Tag ?? (video.ScreenshotUrls[0].Contains(video.ImageUrl) ? 1 : 0);
                _screenshotTimer.Tag = image;
                _screenshotTimer.Start();
            }
        }

        private void previewImage_MouseLeave(object sender, MouseEventArgs e)
        {
            // if ((sender as Image).DataContext is Video video)
            {
                _screenshotTimer.Tag = null;
                _screenshotTimer.Stop();
            }
        }

        private void cmbVideoQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wrapPanel != null)
                SearchVideos();
        }

        private void categoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = categoriesList.SelectedItem as Category;
            if (category != null)
            {
                var items = _videoService.GetVideos(category);
                LoadVideos(items);
                var first = items.FirstOrDefault();
                if (first != null)
                {
                    wrapPanel.ScrollIntoView(first);
                }
            }
        }

        #endregion
    }
}

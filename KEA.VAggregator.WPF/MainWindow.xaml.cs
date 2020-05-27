using KEA.VAggregator.StdLib.Models;
using KEA.VAggregator.StdLib.Services;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KEA.VAggregator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IVideoService _videoService = new TestVideoService();
        private DispatcherTimer _screenshotTimer; // = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();

            _screenshotTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(700), DispatcherPriority.Normal, screenshotTimer_Tick, this.Dispatcher);

            var items = _videoService.GetVideos(); //.GetCategories().OrderBy(c => c.Name);
            wrapPanel.ItemsSource = items;
        }

        private void wrapPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebItem selectedItem = wrapPanel.SelectedItem as WebItem;
            if (selectedItem != null)
            {
                searchInput.Text = selectedItem.Name;              
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var items = _videoService.SearchVideos(searchInput.Text);
            wrapPanel.ItemsSource = items;
        }

        private void searchInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                searchButton_Click(sender, e);
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
            if (selectedItem != null && e.ClickCount > 1)
            {
                if (selectedItem is Video)
                {
                    VideoWindow videoWindow = new VideoWindow(); // { Owner = this };
                    videoWindow.Show();

                    Video video = selectedItem as Video;
                    _videoService.FillVideoUrlsAndInfo(video);
                    //video.PlayUrl = _videoService.GetVideoUrl(video);
                    videoWindow.PlayVideo(video);
                }
                else
                {

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
    }
}

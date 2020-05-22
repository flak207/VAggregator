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

namespace KEA.VAggregator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IVideoService _videoService = new TestVideoService();

        public MainWindow()
        {
            InitializeComponent();

            var items = _videoService.GetVideos(); //.GetCategories().OrderBy(c => c.Name);
            wrapPanel.ItemsSource = items;
        }

        private void wrapPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebItem selectedItem = wrapPanel.SelectedItem as WebItem;
            if (selectedItem != null)
            {
                if (selectedItem is Video)
                {
                    VideoWindow videoWindow = new VideoWindow(); // { Owner = this };
                    videoWindow.Show();

                    Video video = selectedItem as Video;
                    _videoService.FillVideoPlayAndQualityUrls(video);
                    //video.PlayUrl = _videoService.GetVideoUrl(video);
                    videoWindow.PlayVideo(video);
                }
                else
                {

                }
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
    }
}

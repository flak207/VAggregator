using KEA.VAggregator.StdLib.Models;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KEA.VAggregator.WPF
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            mePlayer.MediaOpened += MePlayer_MediaOpened;
        }

        private void MePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        public void PlayVideo(Video video)
        {
            if (!string.IsNullOrWhiteSpace(video?.PlayUrl))
            {
                this.Title = video.Name;
                mePlayer.Source = new Uri(video?.PlayUrl);
                mePlayer.Play();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"),
                        mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }

        private void videoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                var targetPosition = Convert.ToInt32(mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds * videoSlider.Value / 100);
                mePlayer.Position = new TimeSpan(0, 0, 0, 0, targetPosition);
            }
        }

        private void btnToggleScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle == WindowStyle.None)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.Topmost = false;
                btnToggleScreen.Content = "Full Screen";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.Topmost = true;
                btnToggleScreen.Content = "Normal Screen";
            }
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.IsMuted = !mePlayer.IsMuted;
            btnMute.Content = mePlayer.IsMuted ? "Unmute" : "Mute";
        }

      

        private void mainPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            controlsPanel.Visibility = Visibility.Visible;
        }

        private void mainPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            controlsPanel.Visibility = Visibility.Hidden;
        }
    }
}

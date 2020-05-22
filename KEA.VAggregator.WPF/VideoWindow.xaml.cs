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
        private bool _sliderMouseDown = false;
        private MediaState _mediaState = MediaState.Play;
        private Video _video = null;

        public VideoWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public void PlayVideo(Video video)
        {
            if (!string.IsNullOrWhiteSpace(video?.PlayUrl))
            {
                _video = video;
                this.Title = _video.Name;
                cmbQuality.ItemsSource = _video.QualityUrls.Keys.ToList();
                cmbQuality.SelectedItem = _video.QualityUrls.Keys.FirstOrDefault(k => _video.QualityUrls[k] == _video?.PlayUrl);

                mePlayer.Source = new Uri(_video?.PlayUrl);
                mePlayer.IsMuted = true;
                mePlayer.Play();
                _mediaState = MediaState.Play;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                if (!_sliderMouseDown)
                    videoSlider.Value = mePlayer.Position.TotalMilliseconds * 100 / mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                //lblStatus.Text = $"{mePlayer.Position:mm\\:ss} / {mePlayer.NaturalDuration.TimeSpan:mm\\:ss}";
                var position = mePlayer.Position;
                var duration = mePlayer.NaturalDuration.TimeSpan;
                lblStatus.Text = $"{position.TotalMinutes:00}:{position.Seconds:00} / {duration.TotalMinutes:00}:{duration.Seconds:00}";
            }
            else
                lblStatus.Text = "00:00 / 00:00";
        }

        private void btnTogglePlay_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaState == MediaState.Play)
            {
                _mediaState = MediaState.Pause;
                mePlayer.Pause();
                btnTogglePlay.Content = "Play";
            }
            else
            {
                _mediaState = MediaState.Play;
                mePlayer.Play();
                btnTogglePlay.Content = "Pause";
            }
        }

        private void btnToggleScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle == WindowStyle.None)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.Topmost = false;
                btnToggleScreen.Content = "Full";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.Topmost = true;
                btnToggleScreen.Content = "Normal";
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

        private void videoSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _sliderMouseDown = true;
        }

        private void videoSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _sliderMouseDown = false;
            if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                var targetPosition = Convert.ToInt32(mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds * videoSlider.Value / 100);
                mePlayer.Position = new TimeSpan(0, 0, 0, 0, targetPosition);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                switch (e.Key)
                {
                    case Key.Space:
                        btnTogglePlay_Click(sender, e);
                        break;
                    case Key.Left:
                        mePlayer.Position = new TimeSpan(mePlayer.Position.Ticks).Subtract(new TimeSpan(0, 0, 5));
                        break;
                    case Key.Right:
                        mePlayer.Position = new TimeSpan(mePlayer.Position.Ticks).Add(new TimeSpan(0, 0, 5));
                        break;
                    case Key.Up:
                        mePlayer.SpeedRatio += 0.1;
                        break;
                    case Key.Down:
                        if (mePlayer.SpeedRatio > 0)
                            mePlayer.SpeedRatio -= 0.1;
                        break;
                    case Key.Enter:
                        btnToggleScreen_Click(sender, e);
                        break;
                    case Key.Escape:
                        this.WindowStyle = WindowStyle.None;
                        btnToggleScreen_Click(sender, e);
                        mePlayer.SpeedRatio = 1;
                        break;
                }

            }
        }

        private void cmbQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                var position = mePlayer.Position;
                mePlayer.Source = new Uri(_video?.QualityUrls[cmbQuality.SelectedItem?.ToString()]);
                mePlayer.Position = position;
            }
        }
    }
}

using KEA.VAggregator.StdLib.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const int VOLUME_DELTA = 4;

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

            this.Closing += (sender, e) => { mePlayer.Source = null; };
        }

        public void PlayVideo(Video video)
        {
            if (!string.IsNullOrWhiteSpace(video?.PlayLink))
            {
                _video = video;
                this.Title = _video.Name;
                cmbQuality.ItemsSource = _video.QualityLinks.Keys.ToList();
                cmbQuality.SelectedItem = _video.QualityLinks.Keys.FirstOrDefault(k => _video.QualityLinks[k] == _video?.PlayLink);

                mePlayer.Source = new Uri(_video.PlayLink);
                //mePlayer.IsMuted = true;
                mePlayer.Volume = 0;
                mePlayer.Play();
                _mediaState = MediaState.Play;

                cmbQuality.SelectionChanged += cmbQuality_SelectionChanged;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                if (_video.DurationTs.TotalMilliseconds == 0)
                    _video.DurationTs = mePlayer.NaturalDuration.TimeSpan;

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
                mePlayer.Margin = new Thickness(0);
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
                Point clickPoint = e.GetPosition(videoSlider);
                double sliderWidth = videoSlider.ActualWidth;
                double relative = clickPoint.X / sliderWidth;
                videoSlider.Value = relative * 100;

                var targetPosition = Convert.ToInt32(mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds * relative);
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
                    case Key.A:
                        mePlayer.Position = new TimeSpan(mePlayer.Position.Ticks).Subtract(new TimeSpan(0, 0, 5));
                        break;
                    case Key.Right:
                    case Key.S:
                        mePlayer.Position = new TimeSpan(mePlayer.Position.Ticks).Add(new TimeSpan(0, 0, 5));
                        break;
                    case Key.Up:
                        double newVolumeValue = volumeSlider.Value + VOLUME_DELTA;
                        volumeSlider.Value = newVolumeValue > 100 ? 100 : newVolumeValue;
                        break;
                    case Key.Down:
                        newVolumeValue = volumeSlider.Value - VOLUME_DELTA;
                        volumeSlider.Value = newVolumeValue < 0 ? 0 : newVolumeValue;
                        break;
                    case Key.Enter:
                        btnToggleScreen_Click(sender, e);
                        break;
                    case Key.Escape:
                        this.WindowStyle = WindowStyle.None;
                        btnToggleScreen_Click(sender, e);
                        mePlayer.SpeedRatio = 1;
                        break;
                    case Key.Z:
                        if (mePlayer.SpeedRatio > 0)
                            mePlayer.SpeedRatio -= 0.1;
                        break;
                    case Key.X:
                        mePlayer.SpeedRatio += 0.1;
                        break;
                    case Key.C:
                        //if (e.KeyboardDevice.Modifiers != ModifierKeys.Control)
                        mePlayer.SpeedRatio = 1;
                        break;
                    case Key.D:
                        btnDownload_Click(this, null);
                        break;
                    case Key.D1:
                        downloadSlider.LowerValue = videoSlider.Value;
                        break;
                    case Key.D2:
                        downloadSlider.HigherValue = videoSlider.Value;
                        break;
                    case Key.O:
                        OpenFile();
                        break;
                }

            }
        }

        private void cmbQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (mePlayer.Source != null && mePlayer.NaturalDuration.HasTimeSpan)
            {
                var position = mePlayer.Position;

                _video.PlayLink = _video?.QualityLinks[cmbQuality.SelectedItem?.ToString()];
                mePlayer.Source = new Uri(_video.PlayLink);
                mePlayer.Position = position;

                //_video.DurationTs = mePlayer.NaturalDuration.TimeSpan;
            }
        }

        private void btnApp_Click(object sender, RoutedEventArgs e)
        {
            if (mePlayer.Source != null)
            {
                try
                {
                    this.WindowStyle = WindowStyle.None;
                    btnToggleScreen_Click(sender, e);
                    var playerSource = mePlayer.Source;
                    mePlayer.Source = null;

                    var url = playerSource.OriginalString;
                    if (!string.IsNullOrWhiteSpace(playerSource.Query))
                    {
                        url = url.Replace(playerSource.Query, "");
                    }
                    Process.Start(@"C:\Program Files\DAUM\PotPlayer\PotPlayerMini64.exe", url);
                    //D://Program Files//DAUM//PotPlayer//PotPlayerMini64.exe
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex); ;
                }
            }
        }

        private void btnBrowser_Click(object sender, RoutedEventArgs e)
        {
            if (_video.TargetUrl != null)
            {
                try
                {
                    this.WindowStyle = WindowStyle.None;
                    btnToggleScreen_Click(sender, e);
                    var playerSource = mePlayer.Source;
                    mePlayer.Source = null;

                 
                    Process.Start(@"firefox", _video.TargetUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex); ;
                }
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow() { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            mainWindow.LoadVideos(_video.RelatedVideos);
            mainWindow.Show();
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mePlayer.Volume = volumeSlider.Value / 100;
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta != 0)
            {
                int delta = e.Delta > 0 ? VOLUME_DELTA : -VOLUME_DELTA;
                double newVolumeValue = volumeSlider.Value + delta;
                if (newVolumeValue < 0)
                    newVolumeValue = 0;
                if (newVolumeValue > 100)
                    newVolumeValue = 100;

                volumeSlider.Value = newVolumeValue;
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnToggleScreen_Click(sender, e);
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (downloadSlider.Visibility == Visibility.Visible)
            {
                this.WindowStyle = WindowStyle.None;
                btnToggleScreen_Click(sender, e);
                mePlayer.Source = null;

                double lowerValue = downloadSlider.LowerValue;
                double higherValue = downloadSlider.HigherValue;

                double totalMs = _video.DurationTs.TotalMilliseconds;
                double lowerMs = totalMs * lowerValue / 100;
                double higherMs = totalMs * higherValue / 100;

                string startTime = (new TimeSpan(0, 0, 0, 0, (int)lowerMs)).ToString(@"hh\:mm\:ss");
                string endTime = (new TimeSpan(0, 0, 0, 0, (int)higherMs)).ToString(@"hh\:mm\:ss");//"00:00:10";

                string quality = cmbQuality.SelectedItem?.ToString().Split(',')[0];
                string fileName = $"{_video.Name} {quality}.mp4";

                fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName);

                string arguments = $"-ss {startTime} -i {_video.PlayLink} -to {endTime} -c copy -copyts -y \"{fileName}\"";

                ProcessStartInfo startInfo = new ProcessStartInfo("ffmpeg", arguments);
                Process process = Process.Start(startInfo);
                //process.WaitForExit();
            }
            else
            {
                downloadSlider.Visibility = Visibility.Visible;
            }
        }


        Point mousePosition;
        private void mePlayer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(mePlayer);
            Mouse.Capture(mePlayer);
        }

        private void mePlayer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var newPosition = e.GetPosition(mePlayer);
                var margin = mePlayer.Margin;
                margin.Right -= (newPosition.X - mousePosition.X);
                mePlayer.Margin = margin;

                mousePosition = newPosition;
            }
        }

        private void mePlayer_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void OpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog() { Filter = "Video files (*.mp4, *.avi)|*.mp4;*.avi|All files (*.*)|*.*" };
            var dlgResult = dlg.ShowDialog();
            if (dlgResult.HasValue && dlgResult.Value)
            {
                mePlayer.Source = new Uri(dlg.FileName);
            }
        }

       
    }
}

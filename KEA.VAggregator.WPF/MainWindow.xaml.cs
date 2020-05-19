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
            var categories = _videoService.GetCategories();
            categoryPanel.ItemsSource = categories;
        }
    }
}

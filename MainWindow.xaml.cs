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
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Configuration;

namespace WPFCons
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var port = int.Parse(ConfigurationManager.AppSettings.Get("port"));
            var client = new UdpClient(port);

            while (true)
            {
                var data = await client.ReceiveAsync();

                using (var ms = new MemoryStream(data.Buffer))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    Video.Source = image;
                }
            }
        }

        private void ipaddr_Click(object sender, RoutedEventArgs e)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            MessageBox.Show(string.Join("\n", host.AddressList.
                Where(i => i.AddressFamily == AddressFamily.InterNetwork)));
        }
    }
}

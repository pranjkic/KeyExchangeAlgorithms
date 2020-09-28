using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
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

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BindingList<string> messages = new BindingList<string>();

        
        public IKeyExchange RSAKeyExchangeChannel = null;
        public IKeyExchange DiffeHellmanKeyExchangeChannel = null;

        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<IKeyExchange> RSAconnection = new ChannelFactory<IKeyExchange>("RSAKeyExchangeService");
            ChannelFactory<IKeyExchange> DHconnection = new ChannelFactory<IKeyExchange>("DiffeHellmanKeyExchangeService");

            RSAKeyExchangeChannel = RSAconnection.CreateChannel();
            DiffeHellmanKeyExchangeChannel = DHconnection.CreateChannel();

            YourListBox.ItemsSource = messages;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = TextBox.Text;
            if(!string.IsNullOrWhiteSpace(message))
            {
                TextBox.Text = "";
                messages.Add(message);
                if (RSA.IsChecked == true)
                    RSAEncryption.Encrypt(RSAKeyExchangeChannel, message);
                else
                    DiffeHellmanEncryption.Encrypt(DiffeHellmanKeyExchangeChannel, message);
            }
            else
                MessageBox.Show("Please insert message first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

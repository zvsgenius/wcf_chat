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
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;
        int id;
        ServiceChatClient client;
        public MainWindow()
        {
            InitializeComponent();
        }

        void Connect()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                id = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConnDiscon.Content = "Disconnect";
                tbStatusBar.Text = "online";
                tbStatusBar.Foreground = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("Green");
                isConnected = true;
            }
        }

        void Disconnect()
        {
            if (isConnected)
            {
                client.Disconnect(id);
                client = null;
                tbUserName.IsEnabled = true;
                bConnDiscon.Content = "Connect";
                tbStatusBar.Text = "offline";
                tbStatusBar.Foreground = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush)).ConvertFromInvariantString("Red");
                isConnected = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        public void MessageCallback(string message)
        {
            listBox.Items.Add(message);
            listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMessage(tbMessage.Text, id);
                    tbMessage.Text = "";
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.SendMessage(tbMessage.Text, id);
                tbMessage.Text = "";
            }
        }
    }
}

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_LocalChatSqlite
{
    public partial class MainWindow : Window
    {
        private readonly string _nickname;

        public MainWindow(string nickname = "Noname")
        {
            InitializeComponent();
            _nickname = nickname;
            Title = $"Local Chat - {_nickname}";
        }

        private void SendMessage()
        {
            var text = MessageBox.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var display = $"[{System.DateTime.Now:HH:mm}] {_nickname}: {text}";
            MessagesList.Items.Add(display);

            MessageBox.Clear();
            if (MessagesList.Items.Count > 0)
            {
                MessagesList.ScrollIntoView(MessagesList.Items[MessagesList.Items.Count - 1]);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }
    }
}
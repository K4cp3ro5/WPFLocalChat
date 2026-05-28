using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WPF_LocalChatSqlite.Services;
using WPF_LocalChatSqlite.Models;

namespace WPF_LocalChatSqlite
{
    public partial class MainWindow : Window
    {
        private readonly string _nickname;
        private readonly DispatcherTimer _refreshTimer;
        private DateTime _lastLoaded = DateTime.MinValue;

        public MainWindow(string nickname = "Noname")
        {
            InitializeComponent();
            _nickname = nickname;
            Title = $"Local Chat - {_nickname}";

            LoadMessages();

            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(1.5);
            _refreshTimer.Tick += (s, e) => LoadMessages();
            _refreshTimer.Start();
        }

        private void SendMessage()
        {
            var text = MessageBox.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            DatabaseService.SaveMessage(_nickname, text); // do bazy
            MessageBox.Clear();
            LoadMessages();
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

        private void LoadMessages()
        {
            List<ChatMessage> messages;
            try
            {
                messages = DatabaseService.LoadMessages();
            }
            catch
            {
                return;
            }

            if (messages == null) 
                return;

            if (messages.Count > 0 && messages.Last().Timestamp <= _lastLoaded) 
                return;

            MessagesList.Items.Clear();
            foreach (var m in messages)
            {
                var display = $"[{m.Timestamp:HH:mm}] {m.Nickname}: {m.Text}";
                MessagesList.Items.Add(display);
            }

            if (messages.Count > 0)
            {
                _lastLoaded = messages.Last().Timestamp;
                MessagesList.ScrollIntoView(MessagesList.Items[MessagesList.Items.Count - 1]);
            }
        }
    }
}
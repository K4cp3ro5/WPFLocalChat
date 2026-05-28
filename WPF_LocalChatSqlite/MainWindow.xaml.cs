using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPF_LocalChatSqlite.Models;
using WPF_LocalChatSqlite.Services;

namespace WPF_LocalChatSqlite
{
    public partial class MainWindow : Window
    {
        private readonly string _nickname;

        private readonly DispatcherTimer _refreshTimer;

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

            if (string.IsNullOrWhiteSpace(text))
                return;

            DatabaseService.SaveMessage(_nickname, text);

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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn &&
                    btn.Tag is ChatMessage msg)
                {
                    DatabaseService.DeleteMessage(msg.Id);

                    LoadMessages();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void LoadMessages()
        {
            try
            {
                List<ChatMessage> messages =
                    DatabaseService.LoadMessages();

                foreach (var m in messages)
                {
                    m.IsOwnMessage =
                        m.Nickname == _nickname;
                }

                MessagesList.ItemsSource = null;

                MessagesList.ItemsSource = messages;

                if (messages.Count > 0)
                {
                    MessagesList.ScrollIntoView(
                        messages.Last()
                    );
                }
            }
            catch
            {

            }
        }
    }
}
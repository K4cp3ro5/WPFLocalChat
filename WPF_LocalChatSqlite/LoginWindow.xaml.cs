using System.Windows;

namespace WPF_LocalChatSqlite
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var nickname = NicknameBox.Text?.Trim();
            if (string.IsNullOrEmpty(nickname) || nickname.Length < 2)
            {
                ErrorText.Text = "Nickname musi mieć co najmniej 2 znaki.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            var main = new MainWindow(nickname);
            main.Show();
            this.Close();
        }
    }
}

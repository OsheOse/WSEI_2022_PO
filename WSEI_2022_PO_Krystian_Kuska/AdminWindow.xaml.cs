using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace WSEI_2022_PO_Krystian_Kuska
{
    public partial class AdminWindow : Window
    {
        SQLiteAccessUnitTests _sql = new();
        List<PlayerDataModel> players = new();
        public AdminWindow()
        {
            InitializeComponent();
            passwordText.Password = "password";
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usernameText.Text == "admin" && passwordText.Password == "password")
            {
                adminPanel.Visibility = Visibility.Hidden;
                adminPanel.IsEnabled = false;

                LoadPlayers();
            }
        }
        private void LoadPlayers()
        {
            players = _sql.LoadPlayers();
            playersComboBox.Items.Clear();
            foreach (PlayerDataModel player in players)
            {
                playersComboBox.Items.Add(player.Nickname);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlayer = playersComboBox.SelectedItem;
            if (selectedPlayer != null)
            {
                _sql.DeletePlayer(selectedPlayer.ToString());
                playersComboBox.SelectedItem = null;
                LoadPlayers();
            }
        }
    }
}

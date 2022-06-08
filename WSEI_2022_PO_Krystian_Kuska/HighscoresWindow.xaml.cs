using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;


namespace WSEI_2022_PO_Krystian_Kuska
{
    public partial class HighscoresWindow : Window
    {
        private readonly SQLiteAccess _sql = new();
        List<PlayerDataModel> _players = new();
        public HighscoresWindow()
        {
            InitializeComponent();
            LoadPlayers();
        }
        private void LoadPlayers()
        {
            _players = _sql.LoadPlayers();
            int place = 1;
            foreach (PlayerDataModel player in _players)
            {
                highscoresList.Items.Add($"{place}. " + player.Nickname.ToString());
                place++;
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;


namespace WSEI_2022_PO_Krystian_Kuska
{
    public partial class HighscoresWindow : Window
    {
        private readonly SQLiteAccessUnitTests _sql = new();
        List<PlayerDataModel> _players = new();
        public HighscoresWindow()
        {
            InitializeComponent();
            LoadPlayers();
        }
        private void LoadPlayers()
        {
            highscoresList.Items.Clear();
            _players = _sql.LoadPlayers();
            List<PlayerWithScore> playersWithScores = new();
            int place = 1;
            foreach (PlayerDataModel player in _players)
            {
                PlayerWithScore playerWithScore = new();
                playerWithScore.Nickname = player.Nickname.ToString();
                playerWithScore.Score = _sql.LoadPlayersScores(player.ID);
                playersWithScores.Add(playerWithScore);
            }
            var playersToDisplay = playersWithScores.OrderByDescending(s => s.Score);
            foreach (PlayerWithScore playerToDisplay in playersToDisplay)
            {
                highscoresList.Items.Add($"{place}. " + playerToDisplay.Nickname.ToString() + $"     SCORE: {playerToDisplay.Score}");
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

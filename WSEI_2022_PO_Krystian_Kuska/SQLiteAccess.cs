using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace WSEI_2022_PO_Krystian_Kuska
{
    public class SQLiteAccess
    {
        public List<PlayerDataModel> LoadPlayers()
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            var output = conn.Query<PlayerDataModel>("SELECT * FROM PlayerDataModel;");
            return output.ToList();
        }
        public void SavePlayer(PlayerDataModel player, int score)
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            bool doesPlayerExist = conn.Query<int>($"SELECT COUNT(*) FROM PlayerDataModel WHERE Nickname LIKE '{player.Nickname}';").FirstOrDefault() == 0;
            if (!doesPlayerExist)
            {
                CheckIfBetterScore(player, score);
            }
            else
            {
                conn.Execute("INSERT INTO PlayerDataModel (Nickname) VALUES (@Nickname);", player);
                OverrideScore(player, score);
            }
        }
        public void DeletePlayer(string nickname)
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            conn.Execute($"DELETE FORM Players WHERE nickname = {nickname};");
        }
        private void CheckIfBetterScore(PlayerDataModel player, int score)
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            var output = conn.Query<int>($"SELECT s.Score FROM Scores s JOIN PlayerDataModel p ON s.PlayerID = p.ID WHERE p.ID = {player.ID}");
            if (score <= output.FirstOrDefault())
            {
                return;
            }
            else
            {
                OverrideScore(player, score);
            }
        }
        private void OverrideScore(PlayerDataModel player, int newScore)
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            conn.Execute($"UPDATE Scores SET Score = {newScore} WHERE PlayerID = {player.ID};");
        }
        private string LoadConnString(string name = "SnakeConnString")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

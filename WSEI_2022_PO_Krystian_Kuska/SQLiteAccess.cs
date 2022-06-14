using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System;

namespace WSEI_2022_PO_Krystian_Kuska
{
    public class SQLiteAccess
    {
        //Returns list of all players
        public List<PlayerDataModel> LoadPlayers()
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            var output = conn.Query<PlayerDataModel>("SELECT * FROM PlayerDataModel;");
            return output.ToList();
        }

        //Returns score for player of id given in param
        public int LoadPlayersScores(int playerID)
        {
            if (playerID < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(playerID));
            }
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            var output = conn.Query<int>($"SELECT s.Score FROM Scores s JOIN PlayerDataModel p ON s.PlayerID = p.ID WHERE s.PlayerID = {playerID};").FirstOrDefault();
            return output;
        }

        //Checks if player of given name in param exists, if true => checks if player got new highscore, if false => inserts new player to database with their score
        public void SavePlayer(PlayerDataModel player, int score)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            else if (score < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(score));
            }
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            bool doesPlayerExist = conn.Query<int>($"SELECT COUNT(*) FROM PlayerDataModel WHERE Nickname LIKE '{player.Nickname}';").FirstOrDefault() != 0;
            if (doesPlayerExist)
            {
                CheckIfBetterScore(player, score);
            }
            else
            {
                conn.Execute("INSERT INTO PlayerDataModel (Nickname) VALUES (@Nickname);", player);
                int playerID = conn.Query<int>($"SELECT ID FROM PlayerDataModel WHERE Nickname LIKE '{player.Nickname}';").FirstOrDefault();
                conn.Execute($"INSERT INTO Scores (Score, PlayerID) VALUES ({score},{playerID});");
            }
        }

        //Removes player of given nickname in param and their score
        public void DeletePlayer(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrEmpty(nickname))
            {
                throw new ArgumentNullException(nameof(nickname));
            }
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            int playerID = conn.Query<int>($"SELECT ID FROM PlayerDataModel WHERE Nickname = '{nickname}'").FirstOrDefault();
            conn.Execute($"DELETE FROM Scores WHERE PlayerID = {playerID}");
            conn.Execute($"DELETE FROM PlayerDataModel WHERE Nickname = '{nickname}';");
        }

        //Checks if player got better score, if true => overrides their score with the new one, if false => returns
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

        //Overrides score of given player in param with the new value
        private void OverrideScore(PlayerDataModel player, int newScore)
        {
            using IDbConnection conn = new SQLiteConnection(LoadConnString());
            conn.Execute($"UPDATE Scores SET Score = {newScore} WHERE PlayerID = {player.ID};");
        }

        //Loads connection string
        private string LoadConnString(string name = "SnakeConnString")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

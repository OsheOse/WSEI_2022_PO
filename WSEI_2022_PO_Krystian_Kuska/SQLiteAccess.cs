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
        public static List<PlayerModel> LoadPlayers()
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnString()))
            {
                var output = conn.Query<PlayerModel>("SELECT * FROM Players;", new DynamicParameters());
                return output.ToList();
            }
        }
        public static void SavePlayer(PlayerModel player)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnString()))
            {
                conn.Execute("INSERT INTO Players (Nickname) VALUES (@Nickname);", player);
            }
        }
        public static void DeletePlayer(string nickname)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnString()))
            {
                conn.Execute($"DELETE FORM Players WHERE nickname = {nickname};");
            }
        }
        private static string LoadConnString(string name = "SnakeConnString")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

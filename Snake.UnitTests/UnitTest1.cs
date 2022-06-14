using NUnit.Framework;
using System;
using WSEI_2022_PO_Krystian_Kuska;

namespace Snake.UnitTests
{
    public class SnakeUnitTests
    {
        [Test]
        public void LoadPlayerScores_NegativeValue_ReturnsArgumentOutOfRangeException()
        {
            //ARRANGE
            var sqlite = new SQLiteAccess();
            //ACT + ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => sqlite.LoadPlayersScores(-1));
        }
        [Test]
        public void SavePlayer_NullPlayer_ReturnsArgumentNullException()
        {
            //ARRANGE
            var sqlite = new SQLiteAccess();
            //ACT + ASSERT
            Assert.Throws<ArgumentNullException>(() => sqlite.SavePlayer(null, 1));
        }
        [Test]
        public void SavePlayer_NegativeScore_ReturnsArgumentOutOfRangeException()
        {
            //ARRANGE
            var sqlite = new SQLiteAccess();
            PlayerDataModel player = new();
            //ACT + ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => sqlite.SavePlayer(player, -1));
        }
        [Test]
        public void DeletePlayer_EmptyNickname_ReturnsArgumentNullException()
        {
            //ARRANGE
            var sqlite = new SQLiteAccess();
            //ACT + ASSERT
            Assert.Throws<ArgumentNullException>(() => sqlite.DeletePlayer(""));
        }
        [Test]
        public void DeletePlayer_NullNickname_ReturnsArgumentNullException()
        {
            //ARRANGE
            var sqlite = new SQLiteAccess();
            //ACT + ASSERT
            Assert.Throws<ArgumentNullException>(() => sqlite.DeletePlayer(null));
        }
        [Test]
        public void DeletePlayer_WhiteSpacesNickname_ReturnsArgumentNullException()
        {
            //ARRANGE
            var sqlite = new SQLiteAccess();
            //ACT + ASSERT
            Assert.Throws<ArgumentNullException>(() => sqlite.DeletePlayer("           "));
        }
    }
}
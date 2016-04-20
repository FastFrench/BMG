using System;
using BMG_Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BMG_UnitTest
{
	[TestClass]
	public class UnitTestStructures
	{
		[TestMethod]
		public void TestTeams()
		{
			TeamBase team1 = new TeamBase();
			TeamBase team2 = new TeamBase();
			team1.AddPlayer("Test1_1", "Password");
			team1.AddPlayer("Test1_2", "Password");
			team2.AddPlayer("Test2_1", "Password");
			team2.AddPlayer("Test2_2", "Password");
			var px = team2.AddPlayer("Test1_3", "Password");
			var px2 = team2["Test1_3"];
			Assert.IsNotNull(px, "team2.AddPlayer failed");
			Assert.IsNotNull(px2, "access of a player from team failed");
			Assert.AreEqual(px.Login, px2.Login);
			Assert.AreEqual(px.Password, px2.Password);
			Assert.AreEqual(px, px2);
			Assert.IsTrue(team1.MoveExistingPlayerIntoThisTeam(px), "MoveExistingPlayerIntoThisTeam failed");
			var px3 = team2["Test1_3"];
			Assert.IsNull(px3, "Player still in team2, despite move in team1");
			var px4 = team1["Test1_3"];			
			Assert.IsNotNull(px3, "Player not found in team1 after moved");			
		}
	}
}

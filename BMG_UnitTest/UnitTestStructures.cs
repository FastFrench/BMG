using System;
using System.Linq;
using BMG_Structures;
using BMG_Structures.Common;
using BMG_Structures.Troops;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BMG_UnitTest
{
	[TestClass]
	public class UnitTestStructures : TestBase
	{
		[TestInitialize()]
		public void MyTestInitialize()
		{
			Init();
		}

		[TestMethod]
		public void TestTeams()
		{
			Init();
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


		[TestMethod]
		public void TestMapSearch()
		{			
			TroopBase b1_11 = CreateBarbarian(1,1,p1_t1);
			TroopBase b2_11 = CreateBarbarian(2,1, p1_t1);
			TroopBase f1_11 = CreateFly(8, 8, p1_t2);
			TroopBase f2_11 = CreateFly(8, 7, p1_t2);
			//AddObstacles(20);		
			var res2 = bf.GetPlaceablesInCell(new Point(2, 1)).ToList();
			Assert.AreEqual(res2.Count, 1, "Wrong number of troops found");
			Assert.IsTrue(b2_11.Equals(res2[0]), "Proper troop not found");
		}
	}
}

using System;
using BMG_Structures;
using BMG_Structures.Common;
using BMG_Structures.Troops;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BMG_UnitTest
{
	[TestClass]
	public class UnitTestStructures
	{
		PlayerBase p11, p12, p21, p22;
		TeamBase team1, team2;
		TroopTemplateBase barbTemplate;
		TroopTemplateBase flyTemplate;
		BattleFieldBase bf;

		private void Init()
		{
			team1 = new TeamBase();
			team2 = new TeamBase();
			p11 = team1.AddPlayer("Test1_1", "Password");
			p12 = team1.AddPlayer("Test1_2", "Password");
			p21 = team2.AddPlayer("Test2_1", "Password");
			p22 = team2.AddPlayer("Test2_2", "Password");

			bf = new BattleFieldBase();
			bf.Initialize(8, 8);

			barbTemplate = new TroopTemplateBase();
			barbTemplate.Name = "Barbarian";
			barbTemplate.Range = 2;
			barbTemplate.MaxHP = 100;
			barbTemplate.SpaceUsed = 3;
			barbTemplate.Speed = 4;
			barbTemplate.TemplateId = 1;
			barbTemplate.Altitude = AltitudeEnum.Ground;

			flyTemplate = new TroopTemplateBase();
			flyTemplate.Name = "Fly";
			flyTemplate.Range = 1;
			flyTemplate.MaxHP = 20;
			flyTemplate.SpaceUsed = 1;
			flyTemplate.Speed = 6;
			flyTemplate.TemplateId = 1;
			flyTemplate.Altitude = AltitudeEnum.Fly;
		}

		private TroopBase CreateBarbarian(int x, int y, PlayerBase player)
		{
			TroopBase barb = new TroopBase(barbTemplate, player);
			barb.Drop(new Point(x, y));
			return barb;
		}

		private TroopBase CreateFly(int x, int y, PlayerBase player)
		{
			TroopBase fly = new TroopBase(flyTemplate, player);
			fly.Drop(new Point(x, y));
			return fly;
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
			BattleFieldBase bf = new BattleFieldBase();			
			bf.Initialize(8, 8);

			TroopTemplateBase ttb = new TroopTemplateBase();
			ttb.Name = "Barbarian";
			ttb.Range = 2;
			ttb.MaxHP = 100;
			ttb.SpaceUsed = 1;
			ttb.Speed = 4;
			ttb.TemplateId = 1;
			TroopBase troop = new TroopBase();
			troop.Drop(new Point(2, 2));
		}


	}
}

﻿using System;
using BMG_Structures;
using BMG_Structures.Common;
using BMG_Structures.Troops;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BMG_UnitTest
{
	public class TestBase
	{
		protected PlayerBase p1_t1, p2_t1, p1_t2, p2_t2;
		protected TeamBase team1, team2;
		protected TroopTemplateBase barbTemplate;
		protected TroopTemplateBase flyTemplate;
		protected BattleFieldBase bf;
		protected Random rnd = new Random();
		
		protected void Init()
		{
			team1 = new TeamBase();
			team2 = new TeamBase();
			p1_t1 = team1.AddPlayer("Test1_1", "Password");
			p2_t1 = team1.AddPlayer("Test1_2", "Password");
			p1_t2 = team2.AddPlayer("Test2_1", "Password");
			p2_t2 = team2.AddPlayer("Test2_2", "Password");

			bf = new BattleFieldBase();
			bf.Initialize(10, 10);
			bf.Teams.Add(team1);
			bf.Teams.Add(team2);

			barbTemplate = new TroopTemplateBase();
			barbTemplate.Name = "Barbarian";
			barbTemplate.Range = 2;
			barbTemplate.MaxHP = 100;
			barbTemplate.SpaceUsed = 3;
			barbTemplate.Speed = 4;
			barbTemplate.TemplateId = 1;
			barbTemplate.Altitude = AltitudeEnum.Ground;
			barbTemplate.TargetableAltitude = AltitudeEnum.Ground;

			flyTemplate = new TroopTemplateBase();
			flyTemplate.Name = "Fly";
			flyTemplate.Range = 1;
			flyTemplate.MaxHP = 20;
			flyTemplate.SpaceUsed = 1;
			flyTemplate.Speed = 6;
			flyTemplate.TemplateId = 2;
			flyTemplate.Altitude = AltitudeEnum.Fly;
			flyTemplate.TargetableAltitude = AltitudeEnum.Fly | AltitudeEnum.Ground;
		}

		protected TroopBase CreateBarbarian(int x, int y, PlayerBase player)
		{
			TroopBase barb = new TroopBase(barbTemplate, player);
			barb.Drop(new Point(x, y));
			return barb;
		}

		protected TroopBase CreateFly(int x, int y, PlayerBase player)
		{
			TroopBase fly = new TroopBase(flyTemplate, player);
			fly.Drop(new Point(x, y));
			return fly;
		}

		protected void AddRandomObstacle(AltitudeEnum altitude = AltitudeEnum.All)
		{
			bf.Cells.AddObstacle(bf.RandomPoint(), altitude);
		}

		protected void AddObstacles(int nbObst)
		{
			for (int i = 0; i < nbObst; i++)
				AddRandomObstacle();
		}

	}
}

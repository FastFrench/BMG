using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;

namespace BMG_GameData.Troop
{
	public class TroopFactory
	{
		TroopTemplate CreateTemplate(TroopEnum type)
		{
			return new TroopTemplate(type);
		}
		Troop CreateTroop(TroopEnum type)
		{
			return new Troop(CreateTemplate(type));
		}
	}
}

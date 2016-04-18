using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;

namespace BMG_Structures
{
	public class VillageBase
	{
		public List<BuildingBase> Buildings { get; private set; }
		public PlayerBase Player { get; protected set; }
	}
}

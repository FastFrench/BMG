using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;
using BMG_Structures.Troops;

namespace BMG_GameData.Troop
{
	public class Troop : TroopBase
	{
		new public TroopTemplate Template { get { return base.Template as TroopTemplate; } set { base.Template = value; } }
		
		public Troop(TroopTemplate template, Player player) 
			: base(template, player)
		{			
		}
	}
}

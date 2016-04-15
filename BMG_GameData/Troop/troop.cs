using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_GameData.Troop
{
	public class Troop : TroopBase
	{
		public TroopEnum TypeId { get { return (TroopEnum)Template.TemplateId; } }
		
		public Troop(TroopTemplate template)
		{
			Template = template;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_GameData.Troop
{
	public class TroopTemplate : TroopTemplateBase
	{
		public TroopTemplate(TroopEnum type)
		{
			TemplateId = (int)type;
		}
	}
}
	
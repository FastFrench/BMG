using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	public class EffectTemplateBase : TemplateBase
	{
		public enum CategoryEnum { Damage, Heal, Teleport }

		public CategoryEnum Category { get; set; }

		public bool AffectBuildings { get; set; }
		public bool AffectTroops { get; set; }
		public bool AffectFriends { get; set; }
		public bool AffectFoes { get; set; }
		
		// When AreaRadius = 0, means only a single target will be affected
		public int AreaRadius { get; set; }

		public EffectTemplateBase()
			: base()
		{
			Altitude = AltitudeEnum.None;
			Category = CategoryEnum.Damage;
		}
	}
}

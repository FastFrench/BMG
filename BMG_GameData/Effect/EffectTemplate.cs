using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_GameData.Effect
{
	public class EffectTemplate : EffectTemplateBase
	{
		new public enum CategoryEnum { 
			Damage = EffectTemplateBase.CategoryEnum.Damage, 
			Heal = EffectTemplateBase.CategoryEnum.Heal, 
			Teleport = EffectTemplateBase.CategoryEnum.Teleport 
		}

		//new public CategoryEnum Category { 
		//	get { return (CategoryEnum)base.Category; } 
		//	set { base.Category = (EffectTemplateBase.CategoryEnum)value; } }

		public EffectTemplate() : base()
		{

		}

	}
}

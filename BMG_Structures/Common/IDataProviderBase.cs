using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;
using BMG_Structures.Troops;

namespace BMG_Structures.Common		
{
	public interface IDataProviderBase
	{
		List<TroopTemplateBase> TroopTypes { get; }
		List<BuildingTemplateBase> BuildingTypes { get; }
	}
}

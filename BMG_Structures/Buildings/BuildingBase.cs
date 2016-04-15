using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_Structures.Buildings
{
	public class BuildingBase
	{
		public int CurrentHP { get; set; }
		public Point CurrentLocation { get; set; }
		public IAI CurrentAI { get; set; }		
		public BuildingTemplateBase Template { get; protected set; }
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_Structures.Troops
{
	public class TroopTemplateBase : TemplateBase
	{
		public int SpaceUsed { get; set; }

		public int Speed { get;  set; }	
	}
}

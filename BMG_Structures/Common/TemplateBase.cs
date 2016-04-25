using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	public class TemplateBase
	{
		public int TemplateId { get; set; }
		public string Name { get; set; }

		public int Cost { get; set; }
		public int Delay { get; set; }
	
		public int MaxHP { get; set; }
		public int Range { get; set; }

		public int Damage { get; set; }
		public AltitudeEnum Altitude { get; set; }
		public AltitudeEnum TargetableAltitude { get; set; }
	}
}

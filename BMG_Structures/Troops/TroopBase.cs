using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_Structures.Troops
{
	public class TroopBase
	{
		public int CurrentHP { get; set; }
		public Point CurrentLocation { get; set; }
		public IAI CurrentAI { get; set; }
		public TroopTemplateBase Template { get; protected set; }
	}
}

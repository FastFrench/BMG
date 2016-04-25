using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	[Flags]
	public enum AltitudeEnum
	{
		Ground = 1,
		Fly = 2 ,
		Underground = 4,
		All = Ground | Fly | Underground
	}
}

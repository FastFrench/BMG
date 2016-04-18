using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	public class PlaceableBase
	{
		virtual public Point CurrentPosition { get; protected set; }
		virtual public AltitudeEnum Altitude { get; protected set; } 
	}
}

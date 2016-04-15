using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	public struct Point
	{
		public Point(int x, int y) : this()
		{
			X = x;
			Y = y;
		}
		public int X { get; set; }
		public int Y { get; set; }
		static public Point InDeck = new Point(-1, -1);
	}
}

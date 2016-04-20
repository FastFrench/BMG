using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	public struct Point
	{
		public Point(int x, int y)
			: this()
		{
			X = x;
			Y = y;
		}
		public int X { get; set; }
		public int Y { get; set; }
		static public Point InDeck = new Point(-1, -1);

		public static bool operator ==(Point p1, Point p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public bool IsInDeck
		{
			get
			{
				return X == InDeck.X && Y == InDeck.Y;
			}
		}

		public int SquareDistance(Point fromPoint)
		{
			if (IsInDeck || fromPoint.IsInDeck)
				return int.MaxValue;
			return (X - fromPoint.X) * (X - fromPoint.X) + (Y - fromPoint.Y) * (Y - fromPoint.Y);
		}
	}
}

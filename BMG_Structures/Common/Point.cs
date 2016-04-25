using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures.Common
{
	using CoordType = Int32;
	public struct Point : IEquatable<Point>
	{
		public Point(CoordType x, CoordType y)
			: this()
		{
			X = x;
			Y = y;
		}
		public CoordType X { get; set; }
		public CoordType Y { get; set; }
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

		public override int GetHashCode()
		{
			return X << 16 | Y;			
		}

		public bool Equals(Point point)
		{
			return this == point;			
		}

		public override bool Equals(object obj)
		{
			if (obj is Point)
				return this == (Point)obj;
			return false;			
		}
	}
}

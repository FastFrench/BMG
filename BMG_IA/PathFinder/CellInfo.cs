using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;

namespace BMG_IA.PathFinder
{

	public struct CellInfo
	{
		public const int DEFAULT_DISTANCE = 999999;
		public const int MAX_WEIGHT = 50;
		public const int DEFAULT_WEIGHT = 5;

		private int _weight;
		private bool _notWalkable;
		private bool _noLOSAllowed;

		/// <summary>
		/// Gives the movement cost when going through this cell
		/// </summary>
		public int Weight { get { if (_weight == 0) return DEFAULT_WEIGHT; return Weight - 1; } set { _weight = value + 1; } }


		/// <summary>
		/// True when the target can move through this cell
		/// </summary>
		public bool IsWalkable
		{
			get
			{
				return !_notWalkable;
			}
			set
			{
				_notWalkable = !value;
			}
		}

		/// <summary>
		/// True when the target can see through this cell
		/// </summary>		
		public bool AllowLOS
		{
			get
			{
				return !_noLOSAllowed;
			}
			set
			{
				_noLOSAllowed = !value;
			}
		}

		public bool Error { get; set; }

		public CellInfo(bool isWalkable = true, bool allowLOS = true) :
			this()
		{
			//this.x = x;
			//this.y = y;
			this._notWalkable = !isWalkable;
			this._noLOSAllowed = !allowLOS;
		}

		static public CellInfo EmptyCell = new CellInfo(false, false) { Error = true};

	}
}

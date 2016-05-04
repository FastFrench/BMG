using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;

namespace BMG_IA.PathFinder
{
	
	public class CellInfo
	{
		public const int DEFAULT_DISTANCE = 999;
		public const int MAX_WEIGHT = 50;
		public const int DEFAULT_WEIGHT = 5;

		/// <summary>
		/// Gives the movement cost when going through this cell
		/// </summary>
		public int Weight { get; set; }


		/// <summary>
		/// True when the target can move through this cell
		/// </summary>
		public bool IsWalkable { get; set; }

		/// <summary>
		/// True when the target can see through this cell
		/// </summary>		
		public bool AllowLOS { get; set; }				


		public CellInfo()
		{			
			IsWalkable = false;
			AllowLOS = false;
			this.Weight = DEFAULT_WEIGHT;
		}

		public CellInfo(bool isWalkable = true, bool allowLOS = true)
		{
			//this.x = x;
			//this.y = y;
			this.IsWalkable = isWalkable;
			this.AllowLOS = allowLOS;
			this.Weight = DEFAULT_WEIGHT;
		}

		
		
	}
}

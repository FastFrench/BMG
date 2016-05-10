using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_IA.PathFinder
{
	/// <summary>
	/// This struct contents the data that is modified by the PathFinder
	/// </summary>
	public struct PathFinderCellData
	{
		int _distanceSteps { get; set; } // Default value should really be 0 in a struct, 

		public int DistanceSteps
		{
			get
			{
				if (_distanceSteps == 0) return CellInfo.DEFAULT_DISTANCE; 
				return _distanceSteps - 1;
			}
			set
			{
				_distanceSteps = value + 1;
			}
		}
		public bool IsInPath { get; set; }
		//public bool IsInPath2 { get; set; }
	}
}

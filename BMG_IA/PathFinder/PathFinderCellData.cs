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
		public int DistanceSteps { get; set; }
		public bool IsInPath { get; set; }
		//public bool IsInPath2 { get; set; }
	}
}

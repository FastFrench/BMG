using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BMG_UT
{
	/// <summary>
	/// Some ideas to get precise and accurates results from:
	///		http://www.codeproject.com/Articles/61964/Performance-Tests-Precise-Run-Time-Measurements-wi
	/// </summary>
	[TestClass]
	public class Benchmark : TestBase
	{		

		[TestMethod]		
		public void StartTest()
		{
			BMG_Benchmarks.Benchmark.StartTest();
		}

	}
}

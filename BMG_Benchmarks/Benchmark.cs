using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMG_Benchmarks
{
	/// <summary>
	/// Some ideas to get precise and accurates results from:
	///		http://www.codeproject.com/Articles/61964/Performance-Tests-Precise-Run-Time-Measurements-wi
	/// </summary>
	static public class Benchmark
	{
		static public void Initialization()
		{
			Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2); // Uses the second Core or Processor for the Test
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime; // Prevents "Normal" processes from interrupting Threads
			Thread.CurrentThread.Priority = ThreadPriority.Highest; // Prevents "Normal" processes from interrupting this Thread
		}

		static public void BackToNormal()
		{
			Process.GetCurrentProcess().ProcessorAffinity =  new IntPtr(3); // Uses the second Core or Processor for the Test
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal; // Prevents "Normal" processes from interrupting Threads
			Thread.CurrentThread.Priority = ThreadPriority.Normal; // Prevents "Normal" processes from interrupting this Thread
		}

		static public void RunPathFinder(int size)
		{
			BMG_IA.PathFinder.Pathfinder pf = new BMG_IA.PathFinder.Pathfinder(null, size, size);
			bool res = pf.FindPath(0, size * size - 1);
		}

		static public void PathFinding()
		{
			RunPathFinder(10);
		}

		static public void PathFinding50()
		{
			RunPathFinder(50);
		}

		static public void PathFinding100()
		{
			RunPathFinder(100);
		}

		static public void PathFinding200()
		{
			RunPathFinder(200);
		}

		static public void AnyProcessThatNeedToBeBenchmarked()
		{
			int i = 0;
			int res = 1;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i; 
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
			while (i++ < 30)
				res *= i;
			while (i-- > 1)
				res /= i;
		}

		static int count = 0;
		static public long CheckProcessThatNeedToBeBenchmarked(bool silent, Action taskToBenchmark)
		{
			int nbTries = 0;
			Stopwatch sw = Stopwatch.StartNew();
			while (sw.ElapsedMilliseconds < 100)
			{
				taskToBenchmark();
				nbTries++;
			}
			sw.Stop();
			if (!silent)
			{
				string result = string.Format("\t{0}: {1:0.000}ms", ++count, sw.Elapsed.TotalMilliseconds / nbTries);
				Console.WriteLine(result);
			}
			return sw.ElapsedTicks / nbTries;
		}

		static public void RunBenchmark(string title, Action action)
		{
			Initialization();
			Console.WriteLine("*** "+title+" ***");			
			Console.WriteLine("Warm-up");
			Stopwatch sw = Stopwatch.StartNew();
			while (sw.Elapsed.TotalSeconds < 0.5)
				CheckProcessThatNeedToBeBenchmarked(true, action);
			Console.WriteLine("Testing");
			sw = Stopwatch.StartNew();
			long min = long.MaxValue;
			long max = 0;
			int nbTry = 0;
			long cumul = 0;
			while (sw.Elapsed.TotalSeconds < 1)
			{
				long res = CheckProcessThatNeedToBeBenchmarked(false, action);
				if (res > max) max = res;
				if (res < min) min = res;
				cumul += res;
				nbTry++;
			}
			Console.WriteLine("{0} => {1} ticks, {2:0.000}ms, precision: {3:P}", title, cumul / nbTry, (cumul * sw.Elapsed.TotalMilliseconds / nbTry) / sw.Elapsed.Ticks, ((double)max - min) / min);
			Console.WriteLine("");
			BackToNormal();
		}

		static public void StartTest()
		{
			Initialization();
			RunBenchmark("Test", AnyProcessThatNeedToBeBenchmarked);
			RunBenchmark("Path Finder 10x10", PathFinding);
			RunBenchmark("Path Finder 50x50", PathFinding50);
			RunBenchmark("Path Finder 100x100", PathFinding100);
			RunBenchmark("Path Finder 200x200", PathFinding200); 
			BackToNormal();
		}

	}
}

using System.Diagnostics;

namespace SleepingBarber.Utils
{
	public static class ConsoleHelper
	{
		private static readonly object _consoleLock = new object();
		private static readonly Stopwatch _stopwatch = new Stopwatch();
		private static bool _shouldStop = false;

		public static void StartTimer()
		{
			_stopwatch.Start();
		}

		public static long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
		public static bool ShouldStop => _shouldStop;

		public static void WriteCustomerAction(int customerID, string action)
		{
			lock (_consoleLock)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write($"{ElapsedMilliseconds} ");

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"Customer ({customerID}) ");

				Console.ResetColor();
				Console.WriteLine(action);
			}
		}

		public static void WriteBarberAction(int barberID, string action)
		{
			lock (_consoleLock)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write($"{ElapsedMilliseconds} ");

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"Barber ({barberID}) ");

				Console.ResetColor();
				Console.WriteLine(action);
			}
		}

		public static void WriteChairStatus(int emptyChairs, int totalChairs)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write($"{ElapsedMilliseconds} ");

			Console.ResetColor();
			Console.WriteLine($"{emptyChairs} chair{(emptyChairs != 1 ? "s are" : " is")} empty out of {totalChairs}.");
		}
	}
}


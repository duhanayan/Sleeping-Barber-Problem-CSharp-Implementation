namespace SleepingBarber
{
	using SleepingBarber.Models;
	using SleepingBarber.Utils;

	class MainClass
	{
		public static void Main(string[] args)
		{
			int barberCount = args.Length > 0 && int.TryParse(args[0], out int b) ? b : 2;
			int chairCount = args.Length > 1 && int.TryParse(args[1], out int c) ? c : 3;

			Console.WriteLine($"Simulation starting with: {barberCount} Barbers, {chairCount} Chairs.");
			Console.WriteLine($"Note: Each barber has a random shave duration (100-500ms).");

			using BarberShop barberShop = new BarberShop(barberCount, chairCount);
			barberShop.OpenShop();

			ConsoleHelper.StartTimer();
			var random = new Random();

			for (int i = 0; i < 10; i++)
			{
				new Customer(i+1, barberShop).Enter();
				Thread.Sleep(random.Next(20, 200));
			}

			// Give some time for the barber to work
			Thread.Sleep(5000);
			barberShop.CloseShop();
		}
	}
}

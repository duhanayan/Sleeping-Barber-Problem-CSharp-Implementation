namespace SleepingBarber.Models
{
	using SleepingBarber.Utils;

	public class Barber
	{
		public int ID { get; }
		private readonly int _shaveDuration;

		private readonly BarberShop _shop;

		private Thread _thread;

		private readonly CancellationToken _token;


		public Barber(int id, BarberShop shop, CancellationToken token)
		{
			ID = id;
            // Random duration between 100ms and 500ms
			_shaveDuration = new Random().Next(100, 500);
			_shop = shop;
			_token = token;
			_thread = new Thread(Work)
            {
                IsBackground = true
            };
		}

		public void Start() => _thread.Start();

		public void Work()
		{
			ConsoleHelper.WriteBarberAction(ID, "has started working.");

			try
			{
				while (true)
				{
					ConsoleHelper.WriteBarberAction(ID, $"is sleeping.");

					Customer customer = _shop.TakeCustomer(_token);

					ConsoleHelper.WriteCustomerAction(customer.ID, $"woke Barber ({ID}) up.");

					ConsoleHelper.WriteBarberAction(ID, $"has started shaving Customer ({customer.ID}).");;

					Thread.Sleep(_shaveDuration);

					ConsoleHelper.WriteCustomerAction(customer.ID, $"is shaved by Barber ({ID}).");;
				}
			}
			catch (OperationCanceledException)
			{
				ConsoleHelper.WriteBarberAction(ID, "has stopped working.");
			}
		}

		public void Join()
		{
			_thread.Join();
		}
	}
}
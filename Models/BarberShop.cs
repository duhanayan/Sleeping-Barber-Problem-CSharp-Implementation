namespace SleepingBarber.Models
{
	using SleepingBarber.Utils;

	public class BarberShop : IDisposable
	{
		private readonly Barber[] _barbers;
		private readonly Queue<Customer> _customerQueue;
		private readonly object _lockObject = new();
		public object LockObject => _lockObject;

		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		private readonly SemaphoreSlim _customerSemaphore;

		public BarberShop(int barberCount, int numberOfChairs)
		{
			_barbers = new Barber[barberCount];

			for (int i = 0; i < barberCount; ++i)
			{
				_barbers[i] = new Barber(i+1, this, _cts.Token);
			}

			_customerQueue = new Queue<Customer>(numberOfChairs);

			_customerSemaphore = new SemaphoreSlim(0, numberOfChairs);
		}

		public void Dispose()
		{
			_cts.Dispose();
			_customerSemaphore.Dispose();
		}

		public void OpenShop()
		{
			foreach (var barber in _barbers)
            	barber.Start();
		}

		public void CloseShop()
		{
			_cts.Cancel();

			foreach (var barber in _barbers)
				barber.Join();
		}

		public bool TryEnter(Customer customer)
		{
			lock (_lockObject)
			{
				if (_customerQueue.Count >= _customerQueue.Capacity)
					return false;

				_customerQueue.Enqueue(customer);

				ConsoleHelper.WriteCustomerAction(customer.ID, $"sat on the {_customerQueue.Count}. chair.");

				_customerSemaphore.Release();

				PrintStatus();

				return true;
			}
		}

		public Customer TakeCustomer(CancellationToken token)
		{
			_customerSemaphore.Wait(token);

			lock (_lockObject)
			{
				Customer customer = _customerQueue.Dequeue();
				PrintStatus();
				return customer;
			}
		}

		private void PrintStatus()
		{
			int emptyChairs = _customerQueue.Capacity - _customerQueue.Count;
			ConsoleHelper.WriteChairStatus(emptyChairs, _customerQueue.Capacity);
		}
	}
}

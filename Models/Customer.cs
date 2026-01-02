using SleepingBarber.Utils;

namespace SleepingBarber.Models
{
	public class Customer
	{
		public int ID { get; }

		private readonly BarberShop _shop;

		public Customer(int id, BarberShop shop)
		{
			ID = id;
			_shop = shop;
		}

		public void Enter()
		{
			ConsoleHelper.WriteCustomerAction(ID, "arrived.");

			if (_shop.TryEnter(this) == false)
			{
				ConsoleHelper.WriteCustomerAction(ID, "left (no empty chair).");
				return;
			}
		}
	}
}
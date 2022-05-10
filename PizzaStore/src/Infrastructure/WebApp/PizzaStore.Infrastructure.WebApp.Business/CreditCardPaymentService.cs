namespace PizzaStore.Infrastructure.WebApp.Business;

using PizzaStore.WebApp.Application;

public class CreditCardPaymentService : IPaymentService
{
	public Task<bool> Pay(int orderId)
	{
		return Task.FromResult(true);
	}
}
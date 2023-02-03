namespace PizzaStore.WebApp.Application;

public interface IPaymentService
{
	Task<bool> Pay(int orderId);
}
using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        List<Order> GetNewOrders();
        Order GetOrder(int id);
        bool OrderExists(int id);
        bool CreateOrder(Order order);
        bool UpdateOrder(Order order);
        bool DeleteOrder(Order order);
        bool Save();
    }
}

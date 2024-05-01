using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IOrderRepository
    {
        ICollection<Order> GetAllOrders();
        ICollection<Order> GetNewOrders();
        Order GetOrder(int id);
        bool OrderExists(int id);
        bool CreateOrder(Order order);
        bool Save();
    }
}

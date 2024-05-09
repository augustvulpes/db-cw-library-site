using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface IOrderService
    {
        public List<OrderDto> GetAllOrders();
        public List<OrderDto> GetNewOrders();
        public OrderDto GetOrder(int orderId);
        public string CreateOrder(OrderDto orderCreate);
        public string UpdateOrder(int orderId, OrderDto orderUpdate);
        public string DeleteOrder(int orderId);
    }
}

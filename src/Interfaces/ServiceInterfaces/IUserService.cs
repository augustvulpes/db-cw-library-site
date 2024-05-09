using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
        public UserDto GetUser(int userId);
        public List<ReviewDto> GetReviewsByUser(int userId);
        public List<OrderDto> GetOrdersByUser(int userId);
        public string CreateUser(UserDto userCreate);
        public string UpdateUser(int userId, UserDto userUpdate);
        public string DeleteUser(int userId);
    }
}

using LibraryApp.Dto;
using LibraryApp.Models;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        public List<UserDto> GetUsers();
        public UserDto GetUser(string userId);
        public List<ReviewDto> GetReviewsByUser(string userId);
        public List<OrderDto> GetOrdersByUser(string userId);
        public Task<User> Register(UserDto userRegister);
        public Task<User> Login(LoginDto userLogin);
        public string CreateUser(UserDto userCreate);
        public string UpdateUser(string userId, UserDto userUpdate);
        public string DeleteUser(string userId);
    }
}

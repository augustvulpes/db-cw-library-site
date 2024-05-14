using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(string id);
        bool UserExists(string id);
        List<Review> GetReviewsByUser(string id);
        List<Order> GetOrdersByUser(string id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}

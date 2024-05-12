using LibraryApp.Models;

namespace LibraryApp.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(string id);
        bool UserExists(string id);
        ICollection<Review> GetReviewsByUser(string id);
        ICollection<Order> GetOrdersByUser(string id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}

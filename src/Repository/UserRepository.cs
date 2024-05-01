using LibraryApp.Data;
using LibraryApp.Interfaces;
using LibraryApp.Models;

namespace LibraryApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Order> GetOrdersByUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(u => u.Orders).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(u => u.Reviews).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}

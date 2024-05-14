using LibraryApp.Data;
using LibraryApp.Interfaces.RepositoryInterfaces;
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

        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(u => u.Orders).First().ToList();
        }

        public List<Review> GetReviewsByUser(string userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(u => u.Reviews).First().ToList();
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(string id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);

            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);

            return Save();
        }
    }
}

﻿using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool UserExists(int id);
        ICollection<Review> GetReviewsByUser(int id);
        ICollection<Order> GetOrdersByUser(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool Save();
    }
}

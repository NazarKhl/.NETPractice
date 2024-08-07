using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReactApp1.Server.Services
{
    public interface IUserService
    {
        List<User> GetAll();
        User? Get(int id);
        void Add(User user);
        void Delete(int id);
        void Update(User user);
        List<User> GetActiveUsers();
        List<User> GetInactiveUsers();
    }

    public class UserService : IUserService
    {
        private readonly UserDBContext _context;

        public UserService(UserDBContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.Include(u => u.Absences).ToList();
        }

        public User? Get(int id)
        {
            return _context.Users.Include(u => u.Absences).FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            var existingUser = _context.Users.Local.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
            }
            else
            {
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existingUser = _context.Users.Local.FirstOrDefault(u => u.Id == id);

            if (existingUser != null)
            {
                _context.Users.Remove(existingUser);
            }
            else
            {
                var user = _context.Users.Find(id);

                if (user != null)
                {
                    _context.Users.Remove(user);
                }
            }
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            var existingUser = _context.Users.Local.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
            }
            else
            {
                _context.Users.Update(user);
            }
            _context.SaveChanges();
        }

        public List<User> GetActiveUsers()
        {
            return _context.Users.Include(u => u.Absences).Where(u => u.isActive).ToList();
        }

        public List<User> GetInactiveUsers()
        {
            return _context.Users.Include(u => u.Absences).Where(u => !u.isActive).ToList();
        }
    }
}

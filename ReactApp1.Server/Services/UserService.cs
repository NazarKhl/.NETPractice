using ReactApp1.Server.Models;
using System.Xml.Linq;

namespace ReactTestApp.Server.Services
{
    public static class UserService
    {
        static List<User> Users { get; }


        static UserService()
        {
            Users = new List<User>
            {
                new User { Id = 1, Name = "Name1", Email = "abc@ex.ex", isActive = true},
                new User { Id = 2, Name = "Name2", Email = "abc@ex.ex", isActive = false},
                new User { Id = 3, Name = "Name3", Email = "abc@ex.ex", isActive = true},
                new User { Id = 4, Name = "Name4", Email = "abc@ex.ex", isActive = true},
                new User { Id = 5, Name = "Name5", Email = "abc@ex.ex", isActive = false}
            };
        }

        public static List<User> GetAll() => Users;

        public static User? Get(int id) => Users.FirstOrDefault(u => u.Id == id);

        public static void Add(User user)
        {
            user.Id = Users.Count > 0 ? Users.Max(u => u.Id) + 1 : 1;
            Users.Add(user);
        }
        public static void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;

            Users.Remove(user);
        }

        public static void Update(User user)
        {
            var index = Users.FindIndex(u => u.Id == user.Id);
            if (index == -1)
                return;

            Users[index] = user;
        }

        public static List<User> GetActiveUsers() => Users.Where(u => u.isActive).ToList();

        public static List<User> GetInactiveUsers() => Users.Where(u => !u.isActive).ToList();
    }
}
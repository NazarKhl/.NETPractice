using ReactApp1.Server.Models;
using ReactApp1.Server.DTOs;

namespace ReactApp1.Server.Interface
{
    public interface IUserService
    {
        List<UserDTO> GetAll();
        UserDTO? Get(int id);
        void Add(UserDTO userDTO);
        void Delete(int id);
        void Update(UserDTO userDTO);
        List<UserDTO> GetActiveUsers();
        List<UserDTO> GetInactiveUsers();
    }
}

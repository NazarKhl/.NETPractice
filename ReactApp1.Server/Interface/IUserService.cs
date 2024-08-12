using ReactApp1.Server.DTOs;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactApp1.Server.Interface
{
    public interface IUserService
    {
        List<UserDTO> GetAll();
        UserDTO? Get(int id);
        void Add(UserDTO userDTO);
        void Delete(int id);
        void Update(UserUpdateDTO userDTO);
        List<UserDTO> GetActiveUsers();
        List<UserDTO> GetInactiveUsers();
        Task<(List<UserDTO> users, int totalCount)> GetPage(int pageNumber, int pageSize, string? sortColumn = "Id", string? sortDirection = "asc");
        Task<(List<UserDTO> users, int totalCount)> GetFilteredUsers(int? id, string? name, string? email, int pageNumber, int pageSize, string? sortColumn = "Id", string? sortDirection = "asc");
    }
}

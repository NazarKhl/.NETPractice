using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ReactApp1.Server.Models.Absence;

namespace ReactApp1.Service
{
    public class UserService : IUserService
    {
        private const string SORT_ASC_DIR = "asc";
        private const string SORT_DESC_DIR = "desc";
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Absence> _absenceRepository;

        public UserService(IRepository<User> userRepository, IRepository<Absence> absenceRepository)
        {
            _userRepository = userRepository;
            _absenceRepository = absenceRepository;
        }

        public List<UserDTO> GetAll()
        {
            return _userRepository.GetAll()
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    isActive = u.isActive,
                    Absences = u.Absences.Select(a => new AbsenceDTO
                    {
                        Id = a.Id,
                        Type = (AbsenceDTO.AbsenceType)a.Type,
                        Description = a.Description,
                        DateFrom = a.DateFrom,
                        DateTo = a.DateTo
                    }).ToList()
                })
                .ToList();
        }

        public UserDTO? Get(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                isActive = user.isActive,
                Absences = user.Absences.Select(a => new AbsenceDTO
                {
                    Id = a.Id,
                    Type = (AbsenceDTO.AbsenceType)a.Type,
                    Description = a.Description,
                    DateFrom = a.DateFrom,
                    DateTo = a.DateTo
                }).ToList()
            };
        }

        public void Add(UserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                isActive = userDTO.isActive,
                Absences = userDTO.Absences.Select(a => new Absence
                {
                    Type = (AbsenceType)a.Type,
                    Description = a.Description,
                    DateFrom = a.DateFrom ?? default,
                    DateTo = a.DateTo ?? default
                }).ToList()
            };

            _userRepository.Add(user);
        }

        public void Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }

        public void Update(UserUpdateDTO userDTO)
        {
            var user = _userRepository.GetById(userDTO.Id);
            if (user != null)
            {
                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.isActive = userDTO.isActive;

                _userRepository.Update(user);
            }
        }

        public List<UserDTO> GetActiveUsers()
        {
            return _userRepository.GetAll()
                .Where(u => u.isActive)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    isActive = u.isActive,
                    Absences = u.Absences.Select(a => new AbsenceDTO
                    {
                        Id = a.Id,
                        Type = (AbsenceDTO.AbsenceType)a.Type,
                        Description = a.Description,
                        DateFrom = a.DateFrom,
                        DateTo = a.DateTo
                    }).ToList()
                })
                .ToList();
        }

        public List<UserDTO> GetInactiveUsers()
        {
            return _userRepository.GetAll()
                .Where(u => !u.isActive)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    isActive = u.isActive,
                    Absences = u.Absences.Select(a => new AbsenceDTO
                    {
                        Id = a.Id,
                        Type = (AbsenceDTO.AbsenceType)a.Type,
                        Description = a.Description,
                        DateFrom = a.DateFrom,
                        DateTo = a.DateTo
                    }).ToList()
                })
                .ToList();
        }

        public async Task<(List<UserDTO> users, int totalCount)> GetPage(UserPaginationDTO paginationDTO)
        {
            var query = _userRepository.GetAll();

            if (paginationDTO.IdFilter.HasValue)
            {
                query = query.Where(u => u.Id == paginationDTO.IdFilter.Value);
            }

            if (!string.IsNullOrEmpty(paginationDTO.NameFilter))
            {
                query = query.Where(u => u.Name.Contains(paginationDTO.NameFilter));
            }

            if (!string.IsNullOrEmpty(paginationDTO.EmailFilter))
            {
                query = query.Where(u => u.Email.Contains(paginationDTO.EmailFilter));
            }

            var totalCount = await query.CountAsync();

            switch (paginationDTO.SortColumn)
            {
                case nameof(User.Name):
                    query = paginationDTO.SortDirection?.ToLower() == SORT_ASC_DIR
                        ? query.OrderBy(u => u.Name)
                        : query.OrderByDescending(u => u.Name);
                    break;
                case nameof(User.Email):
                    query = paginationDTO.SortDirection?.ToLower() == SORT_ASC_DIR
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email);
                    break;
                case nameof(User.Id):
                default:
                    query = paginationDTO.SortDirection?.ToLower() == SORT_ASC_DIR
                        ? query.OrderBy(u => u.Id)
                        : query.OrderByDescending(u => u.Id);
                    break;
            }

            var users = await query
                .Skip((paginationDTO.PageNumber - 1) * paginationDTO.PageSize)
                .Take(paginationDTO.PageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    isActive = u.isActive,
                    Absences = u.Absences.Select(a => new AbsenceDTO
                    {
                        Id = a.Id,
                        Type = (AbsenceDTO.AbsenceType)a.Type,
                        Description = a.Description,
                        DateFrom = a.DateFrom,
                        DateTo = a.DateTo
                    }).ToList()
                })
                .ToListAsync();

            return (users, totalCount);
        }

        public void AddAbsence(int userId, AbsenceDTO absenceDTO)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                var absence = new Absence
                {
                    Type = (AbsenceType)absenceDTO.Type,
                    Description = absenceDTO.Description,
                    DateFrom = absenceDTO.DateFrom ?? default,
                    DateTo = absenceDTO.DateTo ?? default
                };
                user.Absences.Add(absence);
                _userRepository.Update(user);
            }
        }
    }
}

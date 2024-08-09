using ReactApp1.Server.DTOs;
using ReactApp1.Server.Models;
using ReactApp1.Server.Interface;
using System.Collections.Generic;
using System.Linq;
using static ReactApp1.Server.Models.Absence;

namespace ReactApp1.Service
{
    public class UserService : IUserService
    {
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

        public void Update(UserDTO userDTO)
        {
            var user = _userRepository.GetById(userDTO.Id);
            if (user != null)
            {
                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.isActive = userDTO.isActive;

                user.Absences = userDTO.Absences.Select(a => new Absence
                {
                    Type = (AbsenceType)a.Type,
                    Description = a.Description,
                    DateFrom = a.DateFrom ?? default,
                    DateTo = a.DateTo ?? default
                }).ToList();

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
        public void DeleteAbsence(int absenceId)
        {
            var absence = _absenceRepository.GetById(absenceId);
            if (absence != null)
            {
                _absenceRepository.Delete(absence);
            }
        }
    }
}
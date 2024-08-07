using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models;
using ReactApp1.Server.DTOs;
using ReactApp1.Server.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ReactApp1.Service
{
    public class UserService : IUserService
    {
        private readonly UserDBContext _context;

        public UserService(UserDBContext context)
        {
            _context = context;
        }

        public List<UserDTO> GetAll()
        {
            return _context.Users
                .Include(u => u.Absences)
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
            var user = _context.Users
                .Include(u => u.Absences)
                .FirstOrDefault(u => u.Id == id);

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

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Include(u => u.Absences).FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Absences.RemoveRange(user.Absences);
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public void Update(UserDTO userDTO)
        {
            var user = _context.Users.Include(u => u.Absences).FirstOrDefault(u => u.Id == userDTO.Id);
            if (user != null)
            {
                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.isActive = userDTO.isActive;

                _context.Absences.RemoveRange(user.Absences);

                user.Absences = userDTO.Absences.Select(a => new Absence
                {
                    Type = (AbsenceType)a.Type,
                    Description = a.Description,
                    DateFrom = a.DateFrom ?? default,
                    DateTo = a.DateTo ?? default
                }).ToList();

                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public List<UserDTO> GetActiveUsers()
        {
            return _context.Users
                .Include(u => u.Absences)
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
            return _context.Users
                .Include(u => u.Absences)
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
    }
}

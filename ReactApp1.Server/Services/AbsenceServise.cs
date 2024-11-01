﻿using ReactApp1.Server.DTOs;
using ReactApp1.Server.Models;
using ReactApp1.Server.Interface;
using System.Collections.Generic;
using System.Linq;
using static ReactApp1.Server.Models.Absence;
using ReactApp1.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ReactApp1.Service
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Absence> _absenceRepository;
        private readonly UserDBContext dbContext;

        public AbsenceService(IRepository<User> userRepository, IRepository<Absence> absenceRepository, UserDBContext dbContext)
        {
            _userRepository = userRepository;
            _absenceRepository = absenceRepository;
            this.dbContext = dbContext;
        }

        public List<AbsenceDTO> GetAllAbsences()
        {
            return _absenceRepository.GetAll()
                .Select(a => new AbsenceDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Type = (AbsenceDTO.AbsenceType)a.Type,
                    Description = a.Description,
                    DateFrom = a.DateFrom,
                    DateTo = a.DateTo
                }).ToList();
        }


        public List<AbsenceDTO> GetAbsencesByUserId(int userId)
        {
            var user = dbContext.Users.FirstOrDefault(i => i.Id == userId);
            if (user == null) return new List<AbsenceDTO>();

            return _absenceRepository.GetAll().Where(a => a.UserId == userId).Select(a => new AbsenceDTO
            {
                Id = a.Id,
                UserId = a.UserId,
                Type = (AbsenceDTO.AbsenceType)a.Type,
                Description = a.Description,
                DateFrom = a.DateFrom,
                DateTo = a.DateTo
            }).ToList();
        }

        public AbsenceDTO? GetAbsence(int id)
        {
            var absence = _absenceRepository.GetById(id);
            if (absence == null) return null;

            return new AbsenceDTO
            {
                Id = absence.Id,
                UserId = absence.UserId,
                Type = (AbsenceDTO.AbsenceType)absence.Type,
                Description = absence.Description,
                DateFrom = absence.DateFrom,
                DateTo = absence.DateTo
            };
        }

        public void AddAbsence(int userId, AbsenceDTO absenceDTO)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                var absence = new Absence
                {
                    UserId = userId,
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
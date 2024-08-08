using ReactApp1.Server.DTOs;
using System.Collections.Generic;

namespace ReactApp1.Server.Interface
{
    public interface IAbsenceService
    {
        List<AbsenceDTO> GetAbsencesByUserId(int userId);
        AbsenceDTO? GetAbsence(int id);
        void AddAbsence(int userId, AbsenceDTO absenceDTO);
        void DeleteAbsence(int absenceId);
    }
}

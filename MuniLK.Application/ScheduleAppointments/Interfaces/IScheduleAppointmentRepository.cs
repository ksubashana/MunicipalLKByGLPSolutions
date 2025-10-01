using MuniLK.Domain.Entities;

namespace MuniLK.Application.ScheduleAppointment.Interfaces
{
    public interface IScheduleAppointmentRepository
    {
        Task<ScheduleAppointments?> GetByIdAsync(int id);
        Task<IEnumerable<ScheduleAppointments>> GetAllAsync();
        Task<IEnumerable<ScheduleAppointments>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<ScheduleAppointments>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ScheduleAppointments>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task AddAsync(ScheduleAppointments appointment);
        Task UpdateAsync(ScheduleAppointments appointment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
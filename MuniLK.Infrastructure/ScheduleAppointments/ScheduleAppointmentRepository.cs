using Microsoft.EntityFrameworkCore;
using MuniLK.Application.ScheduleAppointment.Interfaces;
using MuniLK.Domain.Entities;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Infrastructure.ScheduleAppointment
{
    public class ScheduleAppointmentRepository : IScheduleAppointmentRepository
    {
        private readonly MuniLKDbContext _context;

        public ScheduleAppointmentRepository(MuniLKDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ScheduleAppointments?> GetByIdAsync(int id)
        {
            return await _context.ScheduleAppointments
                .Include(a => a.OwnerNav)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<IEnumerable<ScheduleAppointments>> GetAllAsync()
        {
            return await _context.ScheduleAppointments
                .Include(a => a.OwnerNav)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleAppointments>> GetByUserIdAsync(Guid userId)
        {
            return await _context.ScheduleAppointments
                .Include(a => a.OwnerNav)
                .Where(a => a.OwnerId == userId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleAppointments>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ScheduleAppointments
                .Include(a => a.OwnerNav)
                .Where(a => a.StartTime >= startDate && a.EndTime <= endDate)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleAppointments>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.ScheduleAppointments
                .Include(a => a.OwnerNav)
                .Where(a => a.OwnerId == userId && a.StartTime >= startDate && a.EndTime <= endDate)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task AddAsync(ScheduleAppointments appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            await _context.ScheduleAppointments.AddAsync(appointment);
            // Note: SaveChanges is handled by UnitOfWork pattern
        }

        public async Task UpdateAsync(ScheduleAppointments appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            _context.ScheduleAppointments.Update(appointment);
            // Note: SaveChanges is handled by UnitOfWork pattern
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.ScheduleAppointments.FindAsync(id);
            if (appointment != null)
            {
                _context.ScheduleAppointments.Remove(appointment);
            }
            // Note: SaveChanges is handled by UnitOfWork pattern
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ScheduleAppointments.AnyAsync(a => a.AppointmentId == id);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using MuniLK.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Security
{
    public class UserRepository : IUserRepository
    {
        private readonly MuniLKDbContext _context;

        public UserRepository(MuniLKDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Contact)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Contact)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Contact)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);
        }

        public async Task LinkContactAsync(Guid userId, Guid contactId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} not found.");

            user.ContactId = contactId;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // NEW: get all users by tenant
        public async Task<List<User>> GetUsersByTenantAsync(Guid tenantId)
        {
            // If global tenant filter is applied, this is still safe and explicit.
            return await _context.Users
                .Where(u => u.TenantId == tenantId)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
    }
}

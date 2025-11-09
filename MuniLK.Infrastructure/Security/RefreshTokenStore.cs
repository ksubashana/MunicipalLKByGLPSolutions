using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using MuniLK.Infrastructure.Data;

namespace MuniLK.Infrastructure.Security
{
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly MuniLKDbContext _ctx;
        public RefreshTokenStore(MuniLKDbContext ctx) => _ctx = ctx;

        public async Task<RefreshToken?> GetByHashAsync(string tokenHash)
            => await _ctx.RefreshTokens.FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

        public async Task<IReadOnlyList<RefreshToken>> GetActiveTokensForUserAsync(Guid userId)
            => await _ctx.RefreshTokens.Where(r => r.UserId == userId && r.RevokedUtc == null && r.ExpiresUtc > DateTime.UtcNow).ToListAsync();

        public async Task<RefreshToken> AddAsync(RefreshToken token)
        {
            _ctx.RefreshTokens.Add(token);
            return token;
        }

        public async Task RevokeAsync(Guid tokenId, Guid? replacedById = null)
        {
            var token = await _ctx.RefreshTokens.FirstOrDefaultAsync(r => r.Id == tokenId);
            if (token == null) return;
            token.RevokedUtc = DateTime.UtcNow;
            token.ReplacedByTokenId = replacedById;
        }

        public Task SaveChangesAsync() => _ctx.SaveChangesAsync();

        public static string Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes);
        }
    }
}

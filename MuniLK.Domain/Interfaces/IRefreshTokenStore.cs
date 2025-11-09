using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MuniLK.Domain.Entities;

namespace MuniLK.Domain.Interfaces
{
    public interface IRefreshTokenStore
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task<IReadOnlyList<RefreshToken>> GetActiveTokensForUserAsync(Guid userId);
        Task<RefreshToken> AddAsync(RefreshToken token);
        Task RevokeAsync(Guid tokenId, Guid? replacedById = null);
        Task SaveChangesAsync();
    }
}

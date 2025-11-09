using System;

namespace MuniLK.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string TokenHash { get; set; } = string.Empty; // SHA256 hash of raw token
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresUtc { get; set; }
        public DateTime? RevokedUtc { get; set; }
        public Guid? ReplacedByTokenId { get; set; }
        public bool IsActive => RevokedUtc == null && DateTime.UtcNow < ExpiresUtc;
    }
}

using Microsoft.EntityFrameworkCore;
using MyApi.Application.Repositories.JWT;
using MyApi.Domain.Entities.JWT;
using MyApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Repositories.JWT
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
       
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            // _context burada zaten base class'tan geliyor
        }

        public async Task<RefreshToken?> GetActiveTokenAsync(Guid userId, string token)
        {
            return await _context.RefreshTokens
             .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token && t.IsActive);
        }

        public async Task<RefreshToken?> GetActiveTokenAsync(string token)
        {
            return await _context.RefreshTokens
             .FirstOrDefaultAsync(t => t.Token == token && t.IsActive);
        }

    }
}

using MyApi.Domain.Entities.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Repositories.JWT
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetActiveTokenAsync(Guid userId, string token);
        Task<RefreshToken?> GetActiveTokenAsync(string token);
    }
}

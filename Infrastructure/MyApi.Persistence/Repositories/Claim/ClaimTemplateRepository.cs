using MyApi.Application.Repositories.Claim;
using MyApi.Domain.Entities.Identity;
using MyApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Repositories.Claim
{
    public class ClaimTemplateRepository : Repository<ClaimTemplate>, IClaimTemplateRepository
    {
        public ClaimTemplateRepository(AppDbContext context) : base(context) { }
    }
}

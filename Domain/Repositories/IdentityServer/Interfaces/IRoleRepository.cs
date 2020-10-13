using Data.Models.IdentityServer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.IdentityServer.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<AppRole>> GetAsync();
        Task<AppRole> GetByIdAsync(string id);
        Task<IdentityResult> CreateAsync(AppRole role);
        Task<IdentityResult> UpdateAsync(Guid id, AppRole role);
        Task<IdentityResult> DeleteAsync(string id);
    }
}

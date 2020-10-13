using Data.Models.IdentityServer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.IdentityServer.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task<IdentityResult> CreateAsync(AppUser user);
        Task<IdentityResult> UpdateAsync(Guid id, AppUser user);
        Task AssignToRolesAsync(Guid id, string roleName);
        Task RemoveRoleToUserAsync(Guid id, string roleName);
        Task<IList<string>> GetUserRolesAsync(string id);
        Task<IdentityResult> DeleteAsync(string id);
    }
}

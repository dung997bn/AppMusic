using Dapper;
using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultilities.Core;

namespace Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        IDbConnectionFactory _connectionFactory = null;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(IDbConnectionFactory connectionFactory, UserManager<AppUser> userManager)
        {
            _connectionFactory = connectionFactory;
            _userManager = userManager;
        }

        public async Task AssignToRolesAsync(Guid id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var normalizedName = roleName.ToUpper();
                var sqlFindRoleId = $"SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = {nameof(normalizedName)}";
                var roleId = await connection.ExecuteScalarAsync<Guid?>(sqlFindRoleId);
                if (!roleId.HasValue)
                {
                    var sqlInsertRole = $"INSERT INTO [AspNetRoles]([Id],[Name], [NormalizedName]) VALUES(@{nameof(roleId)},@{nameof(roleName)}, @{nameof(normalizedName)})";
                    roleId = Guid.NewGuid();
                    await connection.ExecuteAsync(sqlInsertRole, new { roleName, normalizedName });
                }

                var sqlAssignToRole = $@"IF NOT EXISTS(SELECT 1 FROM [AspNetUserRoles] 
                                            WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}) 
                                       INSERT INTO [AspNetUserRoles]([UserId], [RoleId]) VALUES(@userId, @{nameof(roleId)})";

                await connection.ExecuteAsync(sqlAssignToRole, new { userId = user.Id, roleId });
            }
        }

        public async Task<IdentityResult> CreateAsync(AppUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var role = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(role);
        }

        public async Task<IEnumerable<AppUser>> GetAsync()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                var paramaters = new DynamicParameters();
                var result = await connection.QueryAsync<AppUser>("Get_User_All", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetUserRolesAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var model = await _userManager.GetRolesAsync(user);
            return model;
        }

        public async Task RemoveRoleToUserAsync(Guid id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sqlFindRole = "SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName";
                var roleId = await connection.ExecuteScalarAsync<Guid?>(sqlFindRole, new { normalizedName = roleName.ToUpper() });
                if (roleId.HasValue)
                {
                    var sqlDeleteRole = $"DELETE FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}";
                    await connection.ExecuteAsync(sqlDeleteRole, new { userId = user.Id, roleId });
                }
            }
        }

        public async Task<IdentityResult> UpdateAsync(Guid id, AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}

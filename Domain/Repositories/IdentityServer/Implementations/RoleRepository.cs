using Dapper;
using Data.DbContexts.IdentityServer.Core;
using Data.Models.IdentityServer;
using Data.Repositories.IdentityServer.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.IdentityServer.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<AppRole> _roleManager;
        IDbConnectionFactory _connectionFactory = null;

        public RoleRepository(RoleManager<AppRole> roleManager, IDbConnectionFactory connectionFactory)
        {
            _roleManager = roleManager;
            _connectionFactory = connectionFactory;
        }

        public async Task<IdentityResult> CreateAsync(AppRole role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IEnumerable<AppRole>> GetAsync()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                var paramaters = new DynamicParameters();

                var result = await connection.QueryAsync<AppRole>("Get_Role_All", paramaters, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<AppRole> GetByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> UpdateAsync(Guid id, AppRole role)
        {
            role.Id = id;
            return await _roleManager.UpdateAsync(role);
        }
    }
}


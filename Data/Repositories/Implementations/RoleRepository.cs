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

        //public async Task<PagedResult<AppRole>> GetPagingAsync(string keyword, int pageIndex, int pageSize)
        //{
        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        if (conn.State == System.Data.ConnectionState.Closed)
        //            await conn.OpenAsync();

        //        var paramaters = new DynamicParameters();
        //        paramaters.Add("@keyword", keyword);
        //        paramaters.Add("@pageIndex", pageIndex);
        //        paramaters.Add("@pageSize", pageSize);
        //        paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        //        var result = await conn.QueryAsync<AppRole>("Get_Role_AllPaging", paramaters, null, null, System.Data.CommandType.StoredProcedure);

        //        int totalRow = paramaters.Get<int>("@totalRow");

        //        var pagedResult = new PagedResult<AppRole>()
        //        {
        //            Items = result.ToList(),
        //            TotalRow = totalRow,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize
        //        };
        //        return pagedResult;
        //    }
        //}

        public async Task<IdentityResult> UpdateAsync(Guid id, AppRole role)
        {
            role.Id = id;
            return await _roleManager.UpdateAsync(role);
        }
    }
}


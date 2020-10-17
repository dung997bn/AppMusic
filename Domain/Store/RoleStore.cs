using Dapper;
using Data.DbContexts.SqlServer.Core;
using Data.Models.IdentityServer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Store
{
    public class RoleStore : IRoleStore<AppRole>
    {
        IDbConnectionFactory _connectionFactory = null;

        public RoleStore(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                role.Id = Guid.NewGuid();
                var sqlInsert = $@"INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
                    VALUES (@{nameof(AppRole.Id)},@{nameof(AppRole.Name)}, @{nameof(AppRole.NormalizedName)})";
                await connection.ExecuteAsync(sqlInsert, role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();

                var sqlDelete = $"DELETE FROM [AspNetRoles] WHERE [Id] = @{nameof(AppRole.Id)}";
                await connection.ExecuteAsync(sqlDelete, role);
            }

            return IdentityResult.Success;
        }

        public async Task<AppRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = $@"SELECT * FROM [AspNetRoles]
                    WHERE [Id] = @{nameof(roleId)}";
                return await connection.QuerySingleOrDefaultAsync<AppRole>(sql, new { roleId });
            }
        }

        public async Task<AppRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = $@"SELECT * FROM [AspNetRoles]
                    WHERE [NormalizedName] = @{nameof(normalizedRoleName)}";
                return await connection.QuerySingleOrDefaultAsync<AppRole>(sql, new { normalizedRoleName });
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(AppRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = $@"UPDATE [AspNetRoles] SET
                    [Name] = @{nameof(AppRole.Name)},
                    [NormalizedName] = @{nameof(AppRole.NormalizedName)}
                    WHERE [Id] = @{nameof(AppRole.Id)}";
                await connection.ExecuteAsync(sql, role);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}

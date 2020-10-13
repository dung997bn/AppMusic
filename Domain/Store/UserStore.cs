using Dapper;
using Data.DbContexts.IdentityServer.Core;
using Data.Models;
using Data.Models.IdentityServer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Store
{
    public class UserStore : IUserStore<AppUser>, IUserEmailStore<AppUser>, IUserPhoneNumberStore<AppUser>,
            IUserTwoFactorStore<AppUser>, IUserPasswordStore<AppUser>, IUserRoleStore<AppUser>
    {
        IDbConnectionFactory _connectionFactory = null;

        public UserStore(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var normalizedName = roleName.ToUpper();
                var sqlFind = $"SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @{nameof(normalizedName)}";
                var roleId = await connection.ExecuteScalarAsync<Guid?>(sqlFind, new { normalizedName });
                if (!roleId.HasValue)
                {
                    roleId = Guid.NewGuid();
                    var sqlInsert = $"INSERT INTO [AspNetRoles]([Id],[Name], [NormalizedName]) VALUES(@{nameof(roleId)},@{nameof(roleName)}, @{nameof(normalizedName)})";
                    await connection.ExecuteAsync(sqlInsert, new { roleName, normalizedName });
                }

                var sqlAddToRole = $"IF NOT EXISTS(SELECT 1 FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}) " +
                    $"INSERT INTO [AspNetUserRoles]([UserId], [RoleId]) VALUES(@userId, @{nameof(roleId)})";
                await connection.ExecuteAsync(sqlAddToRole, new { userId = user.Id, roleId });
            }
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                user.Id = Guid.NewGuid();
                var sqlInsert = $@"INSERT INTO [AspNetUsers] ([Id],[UserName], [NormalizedUserName], [Email],
                    [NormalizedEmail], [EmailConfirmed], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled],
                    [LockoutEnabled], [AccessFailedCount],[FullName],[Address],[DoB],[CreatedDate],[UpdatedDate],[About])
                    VALUES (@{nameof(AppUser.Id)},@{nameof(AppUser.UserName)}, @{nameof(AppUser.NormalizedUserName)}, @{nameof(AppUser.Email)},
                    @{nameof(AppUser.NormalizedEmail)}, @{nameof(AppUser.EmailConfirmed)}, @{nameof(AppUser.PasswordHash)},
                    @{nameof(AppUser.PhoneNumber)}, @{nameof(AppUser.PhoneNumberConfirmed)}, @{nameof(AppUser.TwoFactorEnabled)},
                    @{nameof(AppUser.LockoutEnabled)}, @{nameof(AppUser.AccessFailedCount)},@{nameof(AppUser.FullName)},@{nameof(AppUser.Adress)},
                    @{nameof(AppUser.DoB)},@{nameof(AppUser.CreatedDate)},@{nameof(AppUser.UpdatedDate)},@{nameof(AppUser.About)})";

                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                await connection.ExecuteAsync(sqlInsert, user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sqlDel = $"DELETE FROM [AspNetUsers] WHERE [Id] = @{nameof(AppUser.Id)}";
                await connection.ExecuteAsync(sqlDel, user);
            }

            return IdentityResult.Success;
        }

        public async Task<AppUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = $@"SELECT * FROM [AspNetUsers]
                    WHERE [NormalizedEmail] = @{nameof(normalizedEmail)}";
                return await connection.QuerySingleOrDefaultAsync<AppUser>(sql, new { normalizedEmail });
            }
        }

        public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = $@"SELECT * FROM [AspNetUsers]
                    WHERE [Id] = @{nameof(userId)}";
                return await connection.QuerySingleOrDefaultAsync<AppUser>(sql, new { userId });
            }
        }

        public async Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = $@"SELECT * FROM [AspNetUsers]
                    WHERE [NormalizedUserName] = @{nameof(normalizedUserName)}";
                return await connection.QuerySingleOrDefaultAsync<AppUser>(sql, new { normalizedUserName });
            }
        }

        public Task<string> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = "SELECT r.[Name] FROM [AspNetRoles] r INNER JOIN [AspNetUserRoles] ur ON ur.[RoleId] = r.Id " +
                    "WHERE ur.UserId = @userId";
                var queryResults = await connection.QueryAsync<string>(sql, new { userId = user.Id });

                return queryResults.ToList();
            }
        }

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = "SELECT u.* FROM [AspNetUsers] u " +
                    "INNER JOIN [AspNetUserRoles] ur ON ur.[UserId] = u.[Id] INNER JOIN [AspNetRoles] r ON r.[Id] = ur.[RoleId] WHERE r.[NormalizedName] = @normalizedName";
                var queryResults = await connection.QueryAsync<AppUser>(sql,
                    new { normalizedName = roleName.ToUpper() });

                return queryResults.ToList();
            }
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sqlFind = "SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName";
                var roleId = await connection.ExecuteScalarAsync<int?>(sqlFind, new { normalizedName = roleName.ToUpper() });
                if (roleId == default(int)) return false;
                var sql = $"SELECT COUNT(*) FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}";
                var matchingRoles = await connection.ExecuteScalarAsync<int>(sql,
                    new { userId = user.Id, roleId });

                return matchingRoles > 0;
            }
        }

        public async Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sqlFind = "SELECT [Id] FROM [AspNetRoles] WHERE [NormalizedName] = @normalizedName";
                var roleId = await connection.ExecuteScalarAsync<int?>(sqlFind, new { normalizedName = roleName.ToUpper() });
                if (!roleId.HasValue)
                {
                    var sqlDel = $"DELETE FROM [AspNetUserRoles] WHERE [UserId] = @userId AND [RoleId] = @{nameof(roleId)}";
                    await connection.ExecuteAsync(sqlDel, new { userId = user.Id, roleId });
                }
            }
        }

        public Task SetEmailAsync(AppUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(AppUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(AppUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                var sqlUpdate = $@"UPDATE [AspNetUsers] SET
                    [UserName] = @{nameof(AppUser.UserName)},
                    [NormalizedUserName] = @{nameof(AppUser.NormalizedUserName)},
                    [Email] = @{nameof(AppUser.Email)},
                    [NormalizedEmail] = @{nameof(AppUser.NormalizedEmail)},
                    [EmailConfirmed] = @{nameof(AppUser.EmailConfirmed)},
                    [PasswordHash] = @{nameof(AppUser.PasswordHash)},
                    [PhoneNumber] = @{nameof(AppUser.PhoneNumber)},
                    [PhoneNumberConfirmed] = @{nameof(AppUser.PhoneNumberConfirmed)},
                    [TwoFactorEnabled] = @{nameof(AppUser.TwoFactorEnabled)},
                    [DoB] = @{nameof(AppUser.DoB)},
                    [CreatedDate] = @{nameof(AppUser.CreatedDate)},
                    [UpdatedDate] = @{nameof(AppUser.UpdatedDate)},
                    [About]= @{nameof(AppUser.About)}
                    WHERE [Id] = @{nameof(AppUser.Id)}";
                await connection.ExecuteAsync(sqlUpdate, user);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}

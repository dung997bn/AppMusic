using Dapper;
using Data.DbContexts.SqlServer.Core;
using Data.Models.Business;
using Data.Repositories.BusinessServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.BusinessServer.Implementations
{
    public class AudioServerRepository : IAudioServerRepository
    {
        IDbConnectionFactory _connectionFactory;

        public AudioServerRepository(IDbConnectionFactory dbConnection)
        {
            _connectionFactory = dbConnection;
        }

        public async Task<Audio> CreateAudio(Audio audio)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Open();
                    var sqlInsert = $@"INSERT INTO [Audios] ([Id], [UserId], [Name], [Type], [Singer], [UrlMp3], [UrlImage], [Lyrics], [Tags],
                                                    [IsActive], [CreateAt], [UpdatedAt], [DeletedAt])
                                VALUES (@{nameof(audio.Id)}, @{nameof(audio.UserId)}, @{nameof(audio.Name)}, @{nameof(audio.Type)}, @{nameof(audio.Singer)},
                                        @{nameof(audio.UrlMp3)}, @{nameof(audio.UrlImage)}, @{nameof(audio.Lyrics)}, @{nameof(audio.Tags)}, @{nameof(audio.IsActive)},
                                        @{nameof(audio.CreateAt)}, @{nameof(audio.UpdatedAt)}, @{nameof(audio.DeletedAt)})";
                    await connection.ExecuteAsync(sqlInsert, audio);
                    return audio;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Audio> DeleteAudio(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Audio> GetAudioById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Audio>> GetAudios()
        {
            throw new NotImplementedException();
        }
    }
}

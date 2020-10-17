using Dapper;
using Data.DbContexts.SqlServer.Core;
using Data.Models.Business;
using Data.Repositories.BusinessServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.BusinessServer.Implementations
{
    public class VideoServerRepository : IVideoServerRepository
    {
        IDbConnectionFactory _connectionFactory;

        public VideoServerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Video> CreateVideo(Video video)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Open();
                    var sqlInsert = $@"INSERT INTO [Videos] ([Id], [UserId], [Name], [Type], [Author], [UrlMp4], [UrlImage], [Description], [Tags],
                                                    [IsActive], [CreateAt], [UpdatedAt], [DeletedAt])
                                VALUES (@{nameof(video.Id)}, @{nameof(video.UserId)}, @{nameof(video.Name)}, @{nameof(video.Type)}, @{nameof(video.Author)},
                                        @{nameof(video.UrlMp4)}, @{nameof(video.UrlImage)}, @{nameof(video.Description)}, @{nameof(video.Tags)}, 
                                        @{nameof(video.IsActive)}, @{nameof(video.CreateAt)}, @{nameof(video.UpdatedAt)}, @{nameof(video.DeletedAt)})";
                    await connection.ExecuteAsync(sqlInsert, video);
                    return video;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Video> DeleteVideo(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Video> GetVideoById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Video>> GetVideos()
        {
            throw new NotImplementedException();
        }
    }
}

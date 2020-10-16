using AutoMapper;
using Data.Models.Business;
using EventBus.Events;

namespace BusinessConsumerAPI.Mapper
{
    public class MapperConfigs : Profile
    {
        public MapperConfigs()
        {
            CreateMap<Audio, AudioEvent>().ReverseMap();
            CreateMap<Video, VideoEvent>().ReverseMap();
        }
    }
}

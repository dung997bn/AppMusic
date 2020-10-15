using AutoMapper;
using Data.Models.Business;
using EventBus.Events;

namespace BusinessProducerAPI.Mapper
{
    public class MappingConfigs : Profile
    {
        public MappingConfigs()
        {
            CreateMap<Audio, AudioEvent>().ReverseMap();
            CreateMap<Video, VideoEvent>().ReverseMap();
        }
    }
}

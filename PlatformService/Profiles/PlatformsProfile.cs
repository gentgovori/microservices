using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            //Source -> Target
            CreateMap<Platform, PlatformReadDto>(); //per lexim
            CreateMap<PlatformCreateDto,Platform>(); // per krijim
            CreateMap<PlatformReadDto,PlatformPublishedDto>(); // per message queue
        }
    }
}
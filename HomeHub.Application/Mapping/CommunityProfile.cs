using AutoMapper;
using HomeHub.Application.Communities;
using HomeHub.Domain.Entities;

namespace HomeHub.Application.Mapping
{
    public sealed class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<Community, CommunityListItemDto>()
                .ForMember(dest => dest.HomesCount,
                    opt => opt.MapFrom(src => src.Homes.Count));

            CreateMap<Community, CommunityDetailDto>();

            CreateMap<CreateCommunityRequest, Community>();
            CreateMap<UpdateCommunityRequest, Community>();
        }
    }
}

using AutoMapper;
using HomeHub.Application.Homes;
using HomeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Application.Mapping
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            // Request -> Entity
            CreateMap<CreateHomeRequest, Home>()
                .ForMember(d => d.BasePrice, o => o.MapFrom(s => s.Price));

            // Entity -> Response DTO
            CreateMap<Home, HomeDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s.BasePrice))
                .ForMember(d => d.CommunityName, o => o.MapFrom(s => s.Community.Name));

            CreateMap<UpdateHomeRequest, Home>()
                .ForMember(d => d.BasePrice, o => o.MapFrom(s => s.Price));
        }
    }
}

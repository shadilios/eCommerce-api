using AutoMapper;
using eCommerce.Api.Dtos;
using eCommerce.Core.Entities;

namespace eCommerce.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>();
                //.ForMember(m => m.Products, o => o.MapFrom(x => x.Products[0].Name));
        }
    }
}

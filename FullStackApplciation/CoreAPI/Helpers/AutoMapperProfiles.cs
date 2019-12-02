using AutoMapper;
using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;

namespace CoreAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, UserForListDto>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<Person, UserForDetailDto>();
            CreateMap<Photo, PhotosForDetailDto>();
        }
    }
}

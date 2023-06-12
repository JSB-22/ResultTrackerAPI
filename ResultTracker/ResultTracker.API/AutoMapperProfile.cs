using AutoMapper;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Models.Dto;

namespace ResultTracker.API
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<TopicDto, Topic>().ReverseMap();
            CreateMap<AddTopicRequestDto, Topic>().ReverseMap();
            CreateMap<SubjectDto, Subject>().ReverseMap();
            CreateMap<AddSubjectRequestDto, Subject>().ReverseMap();
        }
    }
}

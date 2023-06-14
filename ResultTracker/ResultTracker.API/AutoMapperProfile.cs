using AutoMapper;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Models.Dto;

namespace ResultTracker.API
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<TopicDto, Topic>().ReverseMap().ForMember(dest=>dest.FullName,opt=>opt.MapFrom(src=>$"Year: {src.Year} - {src.Name}"));
            CreateMap<AddTopicRequestDto, Topic>().ReverseMap();
            CreateMap<SubjectDto, Subject>().ReverseMap().ForMember(dest=>dest.FullName,opt=>opt.MapFrom(src=>$"{src.ExamBoard} {src.Name}"));
            CreateMap<AddSubjectRequestDto, Subject>().ReverseMap();
            CreateMap<TestResultDto, TestResult>().ReverseMap();
            CreateMap<AddTestResultRequestDto, TestResult>().ReverseMap();
            CreateMap<UpdateTestResultRequestDto, TestResult>().ReverseMap();
        }
    }
}

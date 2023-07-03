using AutoMapper;
using ResultTracker.API.Models.Domain;
using ResultTracker.API.Models.Dto;
using ResultTracker.API.Users;
using ResultTracker.API.Users.Domain;

namespace ResultTracker.API
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<TopicDto, Topic>().ReverseMap();
            CreateMap<AddTopicRequestDto, Topic>().ReverseMap();
            CreateMap<UpdateTopicRequestDto, Topic>().ReverseMap();
            CreateMap<SubjectDto, Subject>().ReverseMap();
            CreateMap<AddSubjectRequestDto, Subject>().ReverseMap();
            CreateMap<UpdateSubjectRequestDto, Subject>().ReverseMap();
            CreateMap<TestResultDto, TestResult>().ReverseMap().ForMember(dest=>dest.Account,opt=>opt.MapFrom(src=>src.Student));
            CreateMap<AddTestResultRequestDto, TestResult>().ReverseMap();
            CreateMap<UpdateTestResultRequestDto, TestResult>().ReverseMap();

            CreateMap<AccountDto,Account>().ReverseMap()
                .ForMember(dest=>dest.StudentName,opt=>opt.MapFrom(src=>src.FullName))
                .ForMember(dest=>dest.TeacherName,opt=>opt.MapFrom(src=>src.Teacher.FullName));
        }
    }
}

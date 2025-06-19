using AutoMapper;
using CodeReviewAPI.Models;
using CodeReviewAPI.DTOs;

namespace CodeReviewAPI.Profiles
{
    public class CodeReviewProfile : Profile
    {
        public CodeReviewProfile()
        {
            // User Mapping
            CreateMap<User, string>()
                .ConvertUsing(src => src.Username);

            // CodeSubmission Mapping
            CreateMap<CodeSubmission, CodeSubmissionDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));

            CreateMap<CreateCodeReviewDto, CodeSubmission>()
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<CreateCodeSubmissionDto, CodeSubmission>()
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // CodeReview Mapping
            CreateMap<CodeReview, CodeReviewDto>();

            // Not needed: CodeReview creation is handled by LLM

            // Optional: Update scenarios (if needed later)
            CreateMap<CreateCodeReviewDto, CodeSubmission>()
                .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore());

            // RegisterDto -> User (excluding ConfirmPassword)
            CreateMap<RegisterDto, User>();
        }
    }
}

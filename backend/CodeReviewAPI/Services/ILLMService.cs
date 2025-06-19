using CodeReviewAPI.Models;
using System.Threading.Tasks;

namespace CodeReviewAPI.Services
{
    public interface ILLMService
    {
        Task<CodeReview> AnalyzeCode(CodeSubmission submission);
    }
}

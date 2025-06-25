using CodeReviewAPI.Models;
using System.Threading.Tasks;

namespace CodeReviewAPI.Services
{
    public interface ILLMService
    {
        Task<CodeReview> AnalyzeCode(CodeSubmission submission);
        Task<string> ContinueReview(CodeSubmission submission, string question);

    }
}

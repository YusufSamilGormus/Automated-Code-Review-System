using CodeReviewAPI.Models;
using CodeReviewAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewAPI.Services
{
    public interface ICodeReviewService
    {
        Task<CodeSubmission> SubmitCodeForReview(int userId, CreateCodeSubmissionDto request);
        Task<IEnumerable<CodeSubmission>> GetUserSubmissions(int userId);
        Task<CodeSubmission?> GetSubmission(int submissionId, int userId);
        Task SaveCodeReview(CodeReview review);
        Task<string> ContinueReviewWithLLM(int submissionId, string question);

        Task DeleteSubmission(CodeSubmission submission); 
        Task UpdateSubmission(CodeSubmission submission); 
    }
}

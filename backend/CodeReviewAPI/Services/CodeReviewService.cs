using CodeReviewAPI.Data;
using CodeReviewAPI.DTOs;
using CodeReviewAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeReviewAPI.Services
{
    public class CodeReviewService : ICodeReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILLMService _llmService;

        public CodeReviewService(ApplicationDbContext context, ILLMService llmService)
        {
            _context = context;
            _llmService = llmService;
        }

        public async Task<CodeSubmission> SubmitCodeForReview(int userId, CreateCodeSubmissionDto request)
        {
            var submission = new CodeSubmission
            {
                UserId = userId,
                Code = request.Code,
                Language = request.Language,
                Title = request.Title,
                Description = request.Description,
                SubmittedAt = DateTime.UtcNow
            };

            _context.CodeSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return submission;
        }

        public async Task<string> ContinueReviewWithLLM(int submissionId, string question)
        {
            var submission = await _context.CodeSubmissions.FindAsync(submissionId);
            if (submission == null)
                throw new ArgumentException("Submission not found");

            return await _llmService.ContinueReview(submission, question);
        }


        public async Task<IEnumerable<CodeSubmission>> GetUserSubmissions(int userId)
        {
            return await _context.CodeSubmissions
                .Where(s => s.UserId == userId)
                .Include(s => s.Reviews)
                .ToListAsync();
        }

        public async Task<CodeSubmission?> GetSubmission(int submissionId, int userId)
        {
            return await _context.CodeSubmissions
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.Id == submissionId && s.UserId == userId);
        }

        public async Task SaveCodeReview(CodeReview review)
        {
            review.ReviewedAt = DateTime.UtcNow;
            _context.CodeReviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubmission(CodeSubmission submission)
        {
            _context.CodeSubmissions.Remove(submission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubmission(CodeSubmission submission)
        {
            _context.CodeSubmissions.Update(submission);
            await _context.SaveChangesAsync();
        }
    }
}

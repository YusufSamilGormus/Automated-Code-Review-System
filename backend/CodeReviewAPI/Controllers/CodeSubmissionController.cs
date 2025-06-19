using CodeReviewAPI.DTOs;
using CodeReviewAPI.Models;
using CodeReviewAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CodeSubmissionController : ControllerBase
    {
        private readonly ICodeReviewService _reviewService;
        private readonly IMapper _mapper;

        public CodeSubmissionController(ICodeReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // Submit new code
        [HttpPost]
        public async Task<ActionResult<CodeSubmissionDto>> SubmitCode([FromBody] CreateCodeSubmissionDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submission = await _reviewService.SubmitCodeForReview(userId, request);
            return Ok(_mapper.Map<CodeSubmissionDto>(submission));
        }

        // Get all user's submissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeSubmissionDto>>> GetSubmissions()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submissions = await _reviewService.GetUserSubmissions(userId);
            return Ok(_mapper.Map<IEnumerable<CodeSubmissionDto>>(submissions));
        }

        // Get specific submission
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeSubmissionDto>> GetSubmission(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submission = await _reviewService.GetSubmission(id, userId);
            
            if (submission == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CodeSubmissionDto>(submission));
        }

        // Delete submission
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submission = await _reviewService.GetSubmission(id, userId);

            if (submission == null)
            {
                return NotFound();
            }

            await _reviewService.DeleteSubmission(submission);
            return NoContent();
        }

        // Update submission
        [HttpPut("{id}")]
        public async Task<ActionResult<CodeSubmissionDto>> UpdateSubmission(int id, [FromBody] CreateCodeSubmissionDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submission = await _reviewService.GetSubmission(id, userId);

            if (submission == null)
            {
                return NotFound();
            }

            submission.Code = request.Code;
            submission.Language = request.Language;
            submission.Title = request.Title;
            submission.Description = request.Description;

            await _reviewService.UpdateSubmission(submission);
            return Ok(_mapper.Map<CodeSubmissionDto>(submission));
        }
    }
}

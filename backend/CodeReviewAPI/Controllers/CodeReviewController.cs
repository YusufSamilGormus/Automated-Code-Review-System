using CodeReviewAPI.DTOs;
using CodeReviewAPI.Models;
using CodeReviewAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CodeReviewController : ControllerBase
    {
        private readonly ICodeReviewService _reviewService;
        private readonly IMapper _mapper;

        public CodeReviewController(ICodeReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // Submit code for review
        [HttpPost("submit")]
        public async Task<ActionResult<CodeSubmissionDto>> SubmitCodeForReview([FromBody] CreateCodeSubmissionDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submission = await _reviewService.SubmitCodeForReview(userId, request);
            return Ok(_mapper.Map<CodeSubmissionDto>(submission));
        }

        // Get user's code submissions
        [HttpGet("submissions")]
        public async Task<ActionResult<IEnumerable<CodeSubmissionDto>>> GetSubmissions()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var submissions = await _reviewService.GetUserSubmissions(userId);
            return Ok(_mapper.Map<IEnumerable<CodeSubmissionDto>>(submissions));
        }

        // Get specific code submission
        [HttpGet("submission/{id}")]
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
    }
}

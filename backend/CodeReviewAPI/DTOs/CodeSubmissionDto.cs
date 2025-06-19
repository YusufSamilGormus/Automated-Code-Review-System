namespace CodeReviewAPI.DTOs
{
    public class CodeSubmissionDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string Username { get; set; } = string.Empty;
        public ICollection<CodeReviewDto> Reviews { get; set; } = new List<CodeReviewDto>();
    }
}

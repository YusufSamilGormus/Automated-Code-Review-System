namespace CodeReviewAPI.DTOs
{
    public class CreateCodeSubmissionDto
    {
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

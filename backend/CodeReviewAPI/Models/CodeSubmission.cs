namespace CodeReviewAPI.Models
{
    public class CodeSubmission
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        public User User { get; set; } = null!; // null değilse required ilişkisini vurgular

        public ICollection<CodeReview> Reviews { get; set; } = new List<CodeReview>();
    }
}

namespace CodeReviewAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<CodeSubmission> Submissions { get; set; } = new List<CodeSubmission>();

        public string? RefreshToken { get; set; }

        public bool IsEmailVerified { get; set; } = false;

        public string? EmailVerificationToken { get; set; }

        public DateTime? EmailVerificationTokenExpires { get; set; }
    }
}

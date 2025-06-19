namespace CodeReviewAPI.Models
{
    public class CodeReview
    {
        public int Id { get; set; }

        public string Rating { get; set; } = string.Empty;

        public string Strengths { get; set; } = string.Empty;

        public string Suggestions { get; set; } = string.Empty;

        public string BestPractices { get; set; } = string.Empty;

        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

        public string SyntaxErrors { get; set; } = string.Empty;

        public string LogicErrors { get; set; } = string.Empty;

        public string PerformanceIssues { get; set; } = string.Empty;

        public string SecurityIssues { get; set; } = string.Empty;

        public string CodeSmells { get; set; } = string.Empty;

        public string DocumentationIssues { get; set; } = string.Empty;

        public string DesignWeaknesses { get; set; } = string.Empty;

        public string ImplementationWeaknesses { get; set; } = string.Empty;

        public string TestingWeaknesses { get; set; } = string.Empty;

        public string MaintainabilityIssues { get; set; } = string.Empty;

        public int CodeSubmissionId { get; set; }

        public CodeSubmission CodeSubmission { get; set; } = null!;
    }
}

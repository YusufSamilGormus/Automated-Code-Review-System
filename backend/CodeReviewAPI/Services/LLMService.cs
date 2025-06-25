using CodeReviewAPI.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using System.Text.RegularExpressions;

namespace CodeReviewAPI.Services
{
    public class LLMService : ILLMService
    {
        private readonly Kernel _kernel;

        public LLMService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<CodeReview> AnalyzeCode(CodeSubmission submission)
        {
            var prompt = $"""
You are a code review assistant. Please analyze the following {submission.Language} code:

```{submission.Language}
{submission.Code}
```

Provide a detailed review with the following format:
1. Rating (1-5 stars)
2. Strengths
3. Suggestions for improvement
4. Best practices
5. Code mistakes:
   - Syntax errors
   - Logic errors
   - Performance issues
   - Security issues
   - Code smells
   - Documentation issues
6. Structured weaknesses:
   - Design weaknesses
   - Implementation weaknesses
   - Testing weaknesses
   - Maintainability issues
""";

            var response = await _kernel.InvokePromptAsync(prompt);
            var reviewText = response.GetValue<string>();

            var review = new CodeReview
            {
                Rating = ExtractRating(reviewText),
                Strengths = ExtractSection(reviewText, "Strengths"),
                Suggestions = ExtractSection(reviewText, "Suggestions"),
                BestPractices = ExtractSection(reviewText, "Best practices"),
                SyntaxErrors = ExtractSection(reviewText, "Syntax errors"),
                LogicErrors = ExtractSection(reviewText, "Logic errors"),
                PerformanceIssues = ExtractSection(reviewText, "Performance issues"),
                SecurityIssues = ExtractSection(reviewText, "Security issues"),
                CodeSmells = ExtractSection(reviewText, "Code smells"),
                DocumentationIssues = ExtractSection(reviewText, "Documentation issues"),
                DesignWeaknesses = ExtractSection(reviewText, "Design weaknesses"),
                ImplementationWeaknesses = ExtractSection(reviewText, "Implementation weaknesses"),
                TestingWeaknesses = ExtractSection(reviewText, "Testing weaknesses"),
                MaintainabilityIssues = ExtractSection(reviewText, "Maintainability issues")
            };

            return review;
        }

        public async Task<string> ContinueReview(CodeSubmission submission, string question)
        {
            var prompt = $"""
You previously reviewed this {submission.Language} code:

```{submission.Language}
{submission.Code}
```

Now the user has a follow-up question:

"{question}"

Please provide an appropriate answer as a helpful code review assistant.
""";

            var response = await _kernel.InvokePromptAsync(prompt);
            return response.GetValue<string>() ?? "No response from LLM.";
        }

        private string ExtractRating(string text)
        {
            var match = Regex.Match(text, @"\d+\s+stars?");
            return match.Success ? match.Value : "N/A";
        }

        private string ExtractSection(string text, string sectionName)
        {
            var startPattern = $"{sectionName}\\s*:\\s*(\\n|$)";
            var startMatch = Regex.Match(text, startPattern, RegexOptions.IgnoreCase);
            if (!startMatch.Success) return "N/A";

            var startIndex = startMatch.Index + startMatch.Length;
            var remainingText = text.Substring(startIndex);

            var endMatch = Regex.Match(remainingText, @"\d+\.\s+");
            return endMatch.Success ? remainingText.Substring(0, endMatch.Index).Trim() : remainingText.Trim();
        }
    }
}

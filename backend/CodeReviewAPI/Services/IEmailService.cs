namespace CodeReviewAPI.Services
{
	public interface IEmailService
	{
		Task SendVerificationEmailAsync(string toEmail, string token);
	}
}

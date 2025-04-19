using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Auction_System.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _config;

		public EmailSender(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string body)
		{
			var emailSettings = _config.GetSection("EmailSettings");
			var smtpClient = new SmtpClient
			{
				Host = emailSettings["Host"],
				Port = int.Parse(emailSettings["Port"]),
				EnableSsl = bool.Parse(emailSettings["EnableSsl"]),
				Credentials = new NetworkCredential(
					emailSettings["Username"],
					emailSettings["Password"]
				)
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(
					emailSettings["FromEmail"],
					emailSettings["FromName"]
				),
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};

			mailMessage.To.Add(toEmail);
			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}
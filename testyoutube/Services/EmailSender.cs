using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace testyoutube.Services
{
    public class EmailSender : IEmailSender
    {
        /*        private readonly ILogger _logger;

                public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSender> logger)
                {
                    Options = optionsAccessor.Value;
                    _logger = logger;
                }

                public AuthMessageSenderOptions Options { get; set; }

                public async Task SendEmailAsync(string toEmail, string subject, string message)
                { 
                    if (string.IsNullOrEmpty(Options.SendGridKey))
                    {
                        throw new Exception("Null SendGridKey");
                    }
                    await Execute(Options.SendGridKey, subject, message, toEmail);
                }

                public async Task Execute(string apiKey, string subject, string message, string toEmail)
                {
                    var client = new SendGridClient(apiKey);
                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress("romanautomail123@gmail.com", "padhwfaeczfxasgt"),
                        Subject = subject,
                        PlainTextContent = message,
                        HtmlContent = message
                    };
                    msg.AddTo(new EmailAddress(toEmail));

                    // Disable click tracking.
                    // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
                    msg.SetClickTracking(false, false);
                    var response = await client.SendEmailAsync(msg);
                    _logger.LogInformation(response.IsSuccessStatusCode
                                           ? $"Email to {toEmail} queued successfully!"
                                           : $"Failure Email to {toEmail}");
                }*/

            public Task SendMailAsync(string Email, string subject, string message)
            {
                //info de connexion au mail...
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("romanautomail123@gmail.com", "dupbcuegswbwwbdv")
                };

                //creation du mail
                return client.SendMailAsync(
                    new MailMessage(from: "romanautomail123@gmail.com",
                                    to: Email,
                                    subject,
                                    message));
                
            }
       
    }
}

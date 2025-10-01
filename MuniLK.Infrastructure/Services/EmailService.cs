using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MuniLK.Application.Generic.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MuniLK.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendInspectionAssignmentEmailAsync(string toEmail, string inspectorName, string applicationNumber, DateTime assignmentDate, string remarks = null)
        {
            try
            {
                var smtpHost = _emailSettings.SmtpHost;
                var smtpPort = _emailSettings.SmtpPort;
                var smtpUsername = _emailSettings.SmtpUsername;
                var smtpPassword = _emailSettings.SmtpPassword;
                var fromEmail = _emailSettings.FromEmail;
                var fromName = _emailSettings.FromName;

                var subject = $"New Inspection Assignment - {applicationNumber}";
                var body = $@"
Dear {inspectorName},

You have been assigned to perform an inspection for the following application:

Application Number: {applicationNumber}
Scheduled Date: {assignmentDate:dd MMMM yyyy}
Scheduled Time: {assignmentDate:HH:mm}

{(string.IsNullOrWhiteSpace(remarks) ? "" : $"Remarks: {remarks}")}

Please ensure you are available at the scheduled time to complete this inspection.

Best regards,
Municipal System";

                using var smtpClient = new SmtpClient(smtpHost, smtpPort);

                if (!string.IsNullOrWhiteSpace(smtpUsername))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Inspection assignment email sent successfully to {Email} for application {ApplicationNumber}", toEmail, applicationNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send inspection assignment email to {Email} for application {ApplicationNumber}", toEmail, applicationNumber);
                // Don't throw - we don't want email failures to break the assignment process
            }
        }
    }
}
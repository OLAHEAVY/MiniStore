using MiniStore.Core.Interface;
using MiniStore.Utility.Logger;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace MiniStore.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEnvironmentalVariables _environmentalVariables;
        private readonly ILog _logger;
        public EmailService(IEnvironmentalVariables environmentalVariables, ILog logger)
        {
            _environmentalVariables = environmentalVariables;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(_environmentalVariables.SendGridKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridKey, string subject, string htmlMessage, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("minimart@gmail.com", "MiniMart"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));
            try
            {
                return client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogException("An Exception occured while trying to send the email", ex);
            }
            return null;
        }
    }
}

using MailJet.Client;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace ChatUpdater.ApplicationCore.Helpers
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret;

        public string MailjetPublicKey;
        public string MailjetPrivateKey;
        public EmailSender(IConfiguration config)
        {
            SendGridSecret = config.GetValue<string>("SendGridKey");
            MailjetPublicKey = config.GetValue<string>("MailjetPublicKey");
            MailjetPrivateKey = config.GetValue<string>("MailjetPrivateKey");

        }
        public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string htmlMessage)
        {
            EmailProviderType providerType = EmailProviderType.MailJet;
            switch (providerType)
            {
                case EmailProviderType.SendGrid:
                    await SendGridEmail(fromEmail, toEmail, subject, htmlMessage);
                    break;
                case EmailProviderType.MailJet:
                    await MailjetEmail(fromEmail, toEmail, subject, htmlMessage);
                    break;
            }




        }

        public async Task SendGridEmail(string fromEmail, string toEmail, string subject, string htmlMessage)
        {
            SendGridClient Client = new SendGridClient(SendGridSecret);


            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, "Author"),
                Subject = subject,
                PlainTextContent = "",
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(toEmail, subject));
            var res = await Client.SendEmailAsync(msg);
        }

        public async Task MailjetEmail(string fromEmail, string toEmail, string? subject, string? htmlMessage)
        {
            MailJetClient client = new MailJetClient($"{MailjetPublicKey}", $"{MailjetPrivateKey}");


            var message = new MailMessage(fromEmail, toEmail, subject, htmlMessage);
            message.IsBodyHtml = true;

            var res = client.SendMessage(message);
        }
    }

    public enum EmailProviderType
    {
        SendGrid,
        MailJet
    }
    public interface IEmailSender
    {
        public Task SendEmailAsync(string fromEmail, string toEmail, string subject, string htmlMessage);
    }
}

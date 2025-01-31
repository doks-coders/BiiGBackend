using BiiGBackend.Models.SharedModels;
using ChatUpdater.ApplicationCore.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BiiGBackend.Web.Controllers
{
    public class EmailBody
    {
        public string FromEmail { get; set; } = "doks-script-pro@outlook.com";
        public string ToEmail { get; set; } = "guonnie@gmail.com";
        public string Subject { get; set; } = "Testing Email";
        public string HtmlMessage { get; set; } = "<i>Email Tested Successfully</i>";
    }
    public class EmailController(IEmailSender emailSender) : BaseController
    {
        [HttpPost("send-email")]
        public async Task<ActionResult> SendEmail()
        {
            try
            {
                EmailBody body = new();
                await emailSender.SendEmailAsync(body.FromEmail, body.ToEmail, body.Subject, body.HtmlMessage);

            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
            return Ok("Messsage Sent Successfullly");
        }
    }
}

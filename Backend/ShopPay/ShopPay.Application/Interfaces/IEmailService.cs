using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    // New method for templates
    Task SendTemplateEmailAsync(string toEmail, string subject, string templateName, Dictionary<string, string> placeholders);
}

/*public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
*/
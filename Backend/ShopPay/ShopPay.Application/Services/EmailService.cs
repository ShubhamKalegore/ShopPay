using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ShopPay.Application.DTOs;
using ShopPay.Application.Interfaces;
using System.Net;

namespace ShopPay.Application.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendTemplateEmailAsync(string toEmail, string subject, string templateName, Dictionary<string, string> placeholders)
    {
        // 1. Build the path to the template file
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{templateName}.html");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Template {templateName} not found.");

        // 2. Read the template content
        string body = await File.ReadAllTextAsync(templatePath);

        // 3. Replace all placeholders like {{UserName}} with values from the dictionary
        foreach (var item in placeholders)
        {
            body = body.Replace($"{{{{{item.Key}}}}}", item.Value);
        }

        // 4. Use the existing SendEmailAsync method to send the result
        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}

/*public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}
*/
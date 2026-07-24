using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPay.Application.DTOs;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "smtp.gmail.com"; // Default for Gmail
    public int Port { get; set; } = 587;
    public string SenderName { get; set; } // Your App Name (e.g., ShopPay AI)
    public string SenderEmail { get; set; } // Your email address
    public string Password { get; set; } // Your App Password
}
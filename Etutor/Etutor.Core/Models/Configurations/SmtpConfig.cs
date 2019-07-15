using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.Core.Models.Configurations
{
    public class SmtpConfig
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public bool EnableSsl { get; set; }
    }
}

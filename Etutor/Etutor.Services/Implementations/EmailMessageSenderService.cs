using Microsoft.Extensions.Configuration;
using Etutor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Etutor.Core.Models.Configurations;

namespace Etutor.Services.Implementations
{
    public class EmailMessageSenderService : IEmailMessageSenderService
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailMessage _mailMessage;
        private readonly PathsConfig _pathsConfig;
        protected readonly IConfiguration _configuration;

        public EmailMessageSenderService(SmtpClient smtpClient,
                                        MailMessage mailMessage,
                                        IConfiguration configuration)
        {
            _smtpClient = smtpClient;
            _mailMessage = mailMessage;
            _configuration = configuration;
            _pathsConfig = new PathsConfig();
            configuration.GetSection(typeof(PathsConfig).Name).Bind(_pathsConfig);
        }

        public void Add(params string[] to)
        {
            foreach (var address in to)
            {
                _mailMessage.To.Add(address);
            }
        }

        public void SetMailMessage(Dictionary<string, string> keyValuePairs, string templateName,
                                   string childTemplateName = null, params string[] to)
        {
            var HtmlParentView = ReadTemplateFile(templateName);

            if (!string.IsNullOrEmpty(childTemplateName))
            {
                var HtmlChildView = ReadTemplateFile(childTemplateName);
                foreach (var keyValuePair in keyValuePairs)
                {
                    HtmlChildView = HtmlChildView.Replace(keyValuePair.Key, keyValuePair.Value);
                }
                HtmlParentView = HtmlParentView.Replace("{{BODY}}", HtmlChildView);
            }
            else
            {
                foreach (var keyValuePair in keyValuePairs)
                {
                    HtmlParentView = HtmlParentView.Replace(keyValuePair.Key, keyValuePair.Value);
                }
            }

            HtmlParentView = HtmlParentView.Replace("{{PORTAL_NAME}}", _configuration.GetValue<string>("PortalName"));
            HtmlParentView = HtmlParentView.Replace("{{PORTAL_URL}}", _pathsConfig.PortalUrl);

            Add(to);
            _mailMessage.SubjectEncoding = Encoding.UTF8;
            _mailMessage.Subject = keyValuePairs?.GetValueOrDefault("{{SUBJECT}}") ?? string.Empty;
            _mailMessage.Body = HtmlParentView;
        }

        public async Task<bool> SendMessageAsync()
        {
            try
            {
                await _smtpClient.SendMailAsync(_mailMessage);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendMessageAsync(Dictionary<string, string> keyValuePairs, string templateName,
                                                string childTemplateName = null, params string[] to)
        {
            SetMailMessage(keyValuePairs, templateName, childTemplateName, to);
            return await SendMessageAsync();
        }

        private string ReadTemplateFile(string templateName)
        {
            string fileSystemPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _pathsConfig.DefaultMediaContent.GetValueOrDefault("templateEmailPath"), templateName);
            if (File.Exists(fileSystemPath))
                return File.ReadAllText(fileSystemPath);

            return string.Empty;
        }

    }
}

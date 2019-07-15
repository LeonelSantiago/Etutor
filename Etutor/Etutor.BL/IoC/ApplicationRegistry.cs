using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StructureMap;
using System.Net;
using System.Net.Mail;
using System.Text;
using Etutor.Core.Models.Configurations;

namespace Etutor.BL.IoC
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry(ServiceProvider serviceProvider)
        {
            var smtpConfig = serviceProvider.GetService<IOptions<SmtpConfig>>();

            Scan(o =>
            {
                o.Assembly("Etutor.BL");
                o.Assembly("Etutor.Services");
                o.WithDefaultConventions();
            });

            For<UnitOfWork.UnitOfWork>().Use<UnitOfWork.UnitOfWork>().ContainerScoped();

            For<SmtpClient>().Use(() => new SmtpClient(smtpConfig.Value.Host, smtpConfig.Value.Port)
            {
                Timeout = smtpConfig.Value.Timeout,
                EnableSsl = smtpConfig.Value.EnableSsl,
                Credentials = new NetworkCredential(smtpConfig.Value.Email, smtpConfig.Value.Password)
            });
            For<MailMessage>().Use(() => new MailMessage()
            {
                From = new MailAddress(smtpConfig.Value.Email, smtpConfig.Value.DisplayName, Encoding.UTF8),
                IsBodyHtml = true
            });
        }
    }
}

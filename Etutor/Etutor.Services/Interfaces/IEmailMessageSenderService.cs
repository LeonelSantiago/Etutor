using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etutor.Services.Interfaces
{
    public interface IEmailMessageSenderService
    {
        void Add(params string[] to);
        void SetMailMessage(Dictionary<string, string> keyValuePairs, string templateName, string childTemplateName = null, params string[] to);
        Task<bool> SendMessageAsync(Dictionary<string, string> keyValuePairs, string templateName, string childTemplateName = null, params string[] to);
        Task<bool> SendMessageAsync();
    }
}

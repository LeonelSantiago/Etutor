using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Etutor.Core.Models;

namespace Etutor.Core.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void SetOperationResultHeader(this HttpResponse response, OperationResult operationResult)
        {
            var jsonSerializer = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            string value = JsonConvert.SerializeObject(operationResult, jsonSerializer);
            response.Headers.Add(typeof(OperationResult).Name.PascalToKebabCase(), value);
        }

    }
}

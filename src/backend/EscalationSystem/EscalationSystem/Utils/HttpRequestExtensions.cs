using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Utils
{
    public static class HttpRequestExtensions
    {
        public static async Task<T> DeserializeBodyAsync<T>(this HttpRequest httpRequest)
        {
            string requestBody = await new StreamReader(httpRequest.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(requestBody);
        }
    }
}

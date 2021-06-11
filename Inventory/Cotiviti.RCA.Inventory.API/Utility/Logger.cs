using Test.Inventory.API.Enums;
using Test.Inventory.API.Interfaces;
using Test.Inventory.API.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Inventory.API.Utility
{
    /// <summary>
    /// A class which implements <see cref="ILogger"/> interface
    /// <para>
    /// This implementation class uses the shared Logging API to log
    /// </para>
    /// </summary>
    public class Logger : ILog
    {
        private readonly IConfiguration _configuration;

        public Logger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Log via calling the shared Logging API
        /// </summary>
        /// <param name="logType">Log type</param>
        /// <param name="additionalInfo">Additional log message</param>
        /// <param name="ex">An instance of <see cref="Exception"/></param>
        public async Task Log(LogTypeEnum logType, string additionalInfo, Exception ex = null)
        {
            var loggerURI = _configuration.GetValue<string>("LoggerURI");

            Log log = new Log
            {
                ApplicationName = _configuration.GetValue<string>("ApplicationName"),
                LoggerType = "database",
                AdditionalInfo = additionalInfo,
                Message = ex != null ? ex.Message : "",
                StackTrace = ex != null && ex.StackTrace != null ? ex.StackTrace : "",
                InnerException = ex != null && ex.InnerException != null ? (ex.InnerException.Message) : "",
                HostName = Environment.MachineName,
                UserName = Environment.UserName
            };

            HttpClientHandler authHandler = new HttpClientHandler()
            {
                Credentials = CredentialCache.DefaultNetworkCredentials
            };
            var json = JsonConvert.SerializeObject(log);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            using (var httpClient = new HttpClient(authHandler))
            {
                httpClient.BaseAddress = new Uri(loggerURI);
                await httpClient.PostAsync(logType.ToString(), stringContent).ConfigureAwait(false);
            }
        }
    }
}

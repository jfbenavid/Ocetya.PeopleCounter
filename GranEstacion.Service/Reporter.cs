namespace GranEstacion.Service
{
    using GranEstacion.Service.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using OpenPop.Pop3;
    using System;
    using System.Threading.Tasks;

    public class Reporter : MailHandler, IReporter
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Reporter(ILogger<Worker> logger, IConfiguration configuration)
            : base(logger, configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task GetAttachedFileAsync()
        {
            Pop3Client pop3 = new Pop3Client();

            try
            {
                string host = _configuration["EmailConfiguration:Host"];
                string user = $"recent:{_configuration["EmailConfiguration:User"]}";
                string pass = _configuration["EmailConfiguration:Pass"];
                int port = _configuration.GetValue<int>("EmailConfiguration:Port");
                int messageNumber = 1;

                pop3.Connect(host, port, true);
                pop3.Authenticate(user, pass);

                var message = await GetEmailContentAsync(messageNumber, ref pop3);

                if (message != null)
                {
                    if (_configuration.GetValue<bool>("EmailConfiguration:DownloadAttachment:Enabled"))
                    {
                        await DownloadFile(message);
                    }

                    await SaveData();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                pop3.Disconnect();
                pop3.Dispose();
            }
        }

        public async Task SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
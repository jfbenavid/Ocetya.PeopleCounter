namespace GranEstacion.Service
{
    using GranEstacion.Service.Interfaces;
    using Limilabs.Client.POP3;
    using Limilabs.Mail;
    using Limilabs.Mail.MIME;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class Reporter : IReporter
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Reporter(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task GetAttachedFile()
        {
            using Pop3 pop3 = new Pop3();

            try
            {
                string host = _configuration["EmailConfiguration:Host"];
                int port = _configuration.GetValue<int>("EmailConfiguration:Port");
                string user = _configuration["EmailConfiguration:User"];
                string pass = _configuration["EmailConfiguration:Pass"];

                pop3.ConnectSSL(host, port);
                pop3.UseBestLogin(user, pass);

                foreach (string uid in pop3.GetAll())
                {
                    IMail email = new MailBuilder()
                        .CreateFromEml(pop3.GetMessageByUID(uid));

                    Console.WriteLine(email.Subject);

                    // save all attachments to disk
                    foreach (MimeData mime in email.Attachments)
                    {
                        mime.Save(mime.SafeFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                pop3.Close();
                pop3.Dispose();
            }
        }

        public async Task SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
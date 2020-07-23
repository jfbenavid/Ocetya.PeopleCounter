namespace GranEstacion.Service
{
    using GranEstacion.Service.Interfaces;
    using MailKit;
    using MailKit.Net.Imap;
    using MailKit.Search;
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
            ImapClient client = new ImapClient();

            try
            {
                string host = _configuration["EmailConfiguration:Host"];
                string user = _configuration["EmailConfiguration:User"];
                string pass = _configuration["EmailConfiguration:Pass"];
                int port = _configuration.GetValue<int>("EmailConfiguration:Port");
                bool downloadAttachments = _configuration.GetValue<bool>("EmailConfiguration:DownloadAttachment:Enabled");

                await client.ConnectAsync(host, port, true);
                await client.AuthenticateAsync(user, pass);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(SearchQuery.FromContains("jfbenavid@hotmail.com").And(SearchQuery.NotSeen));

                foreach (var uid in uids)
                {
                    var message = await inbox.GetMessageAsync(uid);

                    var x = await GetEmailContentImapAsync(message);

                    if (downloadAttachments)
                    {
                        //await DownloadFile(message);
                    }

                    inbox.AddFlags(uid, MessageFlags.Seen, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public async Task SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
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

                await client.ConnectAsync(host, port, true);
                await client.AuthenticateAsync(user, pass);

                var inbox = client.Inbox;

                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(SearchQuery.FromContains("jfbenavid@hotmail.com").And(SearchQuery.NotSeen));

                var messages = await inbox.FetchAsync(uids, MessageSummaryItems.BodyStructure);

                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        if (_configuration.GetValue<bool>("EmailConfiguration:DownloadAttachment:Enabled"))
                        {
                            //await DownloadFile(message);
                        }

                        inbox.AddFlags(message.UniqueId, MessageFlags.Seen, true);

                        await SaveData();
                    }
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
namespace GranEstacion.Service
{
    using GranEstacion.Service.Config;
    using GranEstacion.Service.Interfaces;
    using MailKit;
    using MailKit.Net.Imap;
    using MailKit.Search;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class Reporter : MailHandler, IReporter
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Reporter(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
            : base(logger, serviceScopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task GetAttachedFileAsync()
        {
            ImapClient client = new ImapClient();

            try
            {
                string host = _configuration[ConfigurationKeys.HOST];
                string user = _configuration[ConfigurationKeys.USER];
                string pass = _configuration[ConfigurationKeys.PASSWORD];
                string from = _configuration[ConfigurationKeys.MAIL_FROM];
                int port = _configuration.GetValue<int>(ConfigurationKeys.PORT);

                await client.ConnectAsync(host, port, true);
                await client.AuthenticateAsync(user, pass);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(SearchQuery.FromContains(from).And(SearchQuery.NotSeen));

                foreach (var uid in uids)
                {
                    var message = await inbox.GetMessageAsync(uid);

                    await GetEmailAndSaveDataAsync(message);

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
    }
}
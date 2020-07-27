namespace GranEstacion.Service
{
    using GranEstacion.Service.Config;
    using GranEstacion.Service.Interfaces;
    using MailKit;
    using MailKit.Net.Imap;
    using MailKit.Net.Smtp;
    using MailKit.Search;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using System;
    using System.Threading.Tasks;

    public class Reporter : MailHandler, IReporter
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<MailConfiguration> _mailConfiguration;

        public Reporter(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory, IOptions<MailConfiguration> mailConfiguration)
            : base(logger, serviceScopeFactory)
        {
            _logger = logger;
            _mailConfiguration = mailConfiguration;
        }

        public async Task GetAttachedFileAsync()
        {
            ImapClient client = new ImapClient();

            try
            {
                await client.ConnectAsync(_mailConfiguration.Value.ImapHost, _mailConfiguration.Value.ImapPort, true);
                await client.AuthenticateAsync(_mailConfiguration.Value.User, _mailConfiguration.Value.Password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(
                    SearchQuery
                    .FromContains(_mailConfiguration.Value.EmailsFrom)
                    .And(SearchQuery.NotSeen));

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

        public async Task SendMailAsync(MimeMessage message)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.MessageSent += (sender, args) =>
                     _logger.LogWarning(args.Response);

                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await smtp.ConnectAsync(_mailConfiguration.Value.SmtpHost, _mailConfiguration.Value.SmtpPort, true);
                    await smtp.AuthenticateAsync(_mailConfiguration.Value.User, _mailConfiguration.Value.Password);
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
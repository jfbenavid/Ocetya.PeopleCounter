namespace Ocetya.PeopleCounter.Service
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using MailKit;
    using MailKit.Net.Imap;
    using MailKit.Net.Smtp;
    using MailKit.Search;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using Ocetya.PeopleCounter.Service.Config;
    using Ocetya.PeopleCounter.Service.Interfaces;

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

        public async Task GetAndSaveNewDataAsync()
        {
            ImapClient client = new ImapClient();

            try
            {
                await client.ConnectAsync(_mailConfiguration.Value.ImapHost, _mailConfiguration.Value.ImapPort, true);
                await client.AuthenticateAsync(_mailConfiguration.Value.ImapUser, _mailConfiguration.Value.ImapPassword);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(
                    SearchQuery
                    .FromContains(_mailConfiguration.Value.EmailsFrom)
                    .And(SearchQuery.NotSeen));

                var messages = await inbox.FetchAsync(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Envelope);

                foreach (var msg in messages)
                {
                    await GetEmailAndSaveDataAsync(msg.BodyParts, inbox, msg.UniqueId);

                    await inbox.AddFlagsAsync(msg.UniqueId, MessageFlags.Seen, true);
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
                    await smtp.AuthenticateAsync(_mailConfiguration.Value.SmtpUser, _mailConfiguration.Value.SmtpPassword);
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

        public async Task UploadReportFromDirectoryAsync(string path)
        {
            string[] fileEntries = Directory.GetFiles(path);

            foreach (var fileName in fileEntries)
            {
                await ReadStreamAsync(File.ReadAllBytes(fileName));

                File.Delete(fileName);
            }

            Directory.Delete(path);
        }
    }
}
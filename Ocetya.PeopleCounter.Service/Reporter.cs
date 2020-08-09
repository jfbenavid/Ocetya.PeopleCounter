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
        private readonly ILogger<Worker> logger;
        private readonly MailConfiguration mailConfiguration;

        public Reporter(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory, IOptions<MailConfiguration> mailConfiguration)
            : base(logger, serviceScopeFactory)
        {
            this.logger = logger;
            this.mailConfiguration = mailConfiguration.Value;
        }

        public async Task GetAndSaveNewDataAsync()
        {
            ImapClient client = new ImapClient();

            try
            {
                await client.ConnectAsync(mailConfiguration.ImapHost, mailConfiguration.ImapPort, true);
                await client.AuthenticateAsync(mailConfiguration.ImapUser, mailConfiguration.ImapPassword);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(
                    SearchQuery
                    .FromContains(mailConfiguration.EmailsFrom)
                    .And(SearchQuery.NotSeen)
                    .And(SearchQuery.SubjectContains(mailConfiguration.EmailSubject)));

                var messages = await inbox.FetchAsync(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure | MessageSummaryItems.Envelope);

                foreach (var msg in messages)
                {
                    await GetEmailAndSaveDataAsync(msg.BodyParts, inbox, msg.UniqueId);

                    await inbox.AddFlagsAsync(msg.UniqueId, MessageFlags.Seen, true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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
                     logger.LogWarning(args.Response);

                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await smtp.ConnectAsync(mailConfiguration.SmtpHost, mailConfiguration.SmtpPort, true);
                    await smtp.AuthenticateAsync(mailConfiguration.SmtpUser, mailConfiguration.SmtpPassword);
                    await smtp.SendAsync(message);
                    await smtp.DisconnectAsync(true);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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
        }
    }
}
namespace GranEstacion.Service
{
    using GranEstacion.Service.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using OpenPop.Mime;
    using OpenPop.Mime.Header;
    using OpenPop.Pop3;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public abstract class MailHandler
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public MailHandler(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task<MessageModel> GetEmailContentAsync(int messageNumber, ref Pop3Client pop3)
        {
            Message message = pop3.GetMessage(messageNumber);
            List<MessagePart> attachment = message.FindAllAttachments();

            if (attachment.Count <= 0 || !attachment[0].FileName.EndsWith(".csv"))
            {
                pop3.DeleteMessage(messageNumber);
                return Task.FromResult<MessageModel>(null);
            }

            MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
            MessagePart plainTextPart = message.FindFirstPlainTextVersion();
            MessageHeader header = message.Headers;

            MessageModel result = new MessageModel
            {
                MessageID = header.MessageId == null ? "" : header.MessageId.Trim(),
                FromID = header.From.Address.Trim(),
                FromName = header.From.DisplayName.Trim(),
                Subject = header.Subject.Trim(),
                Body = (plainTextPart == null ? "" : plainTextPart.GetBodyAsText().Trim()),
                Html = (HTMLTextPart == null ? "" : HTMLTextPart.GetBodyAsText().Trim())
            };

            if (attachment.Count > 0)
            {
                result.FileName = attachment[0].FileName.Trim();
                result.Attachment = attachment;
            }

            return Task.FromResult(result);
        }

        public async Task DownloadFile(MessageModel message)
        {
            IList<MessagePart> attachments = message.Attachment;

            try
            {
                foreach (var attachment in attachments)
                {
                    byte[] content = attachment.Body;
                    await File.WriteAllBytesAsync($"{_configuration["EmailConfiguration:DownloadAttachment:Path"]}/{message.FileName}", attachment.Body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
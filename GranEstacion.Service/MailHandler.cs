namespace GranEstacion.Service
{
    using GranEstacion.Service.Models;
    using MailKit;
    using MailKit.Net.Imap;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MimeKit;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        public async Task<ImapMessage> GetEmailContentImapAsync(MimeMessage message)
        {
            if (message.Attachments.ToList().Count <= 0)
            {
                return null;
            }

            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart part)
                {
                    var fileName = part.FileName;
                    if (!fileName.EndsWith(".csv"))
                        continue;

                    var bytes = await GetBytesArrayToRead(part);

                    using var stream = new MemoryStream(bytes);
                    using var reader = new StreamReader(stream);
                    string line = string.Empty;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            return null;
        }

        private async Task<byte[]> GetBytesArrayToRead(MimePart attachment)
        {
            using var stream = new MemoryStream();
            await attachment.Content.DecodeToAsync(stream);

            return stream.ToArray();
        }
    }
}
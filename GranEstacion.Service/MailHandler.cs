namespace GranEstacion.Service
{
    using MimeKit;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class MailHandler
    {
        protected async Task GetEmailContentImapAsync(MimeMessage message)
        {
            if (message.Attachments.ToList().Count <= 0)
            {
                return;
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
        }

        private async Task<byte[]> GetBytesArrayToRead(MimePart attachment)
        {
            using var stream = new MemoryStream();
            await attachment.Content.DecodeToAsync(stream);

            return stream.ToArray();
        }
    }
}
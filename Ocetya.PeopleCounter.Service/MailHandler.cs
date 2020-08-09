namespace Ocetya.PeopleCounter.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using MailKit;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MimeKit;
    using Ocetya.PeopleCounter.Repository;
    using Ocetya.PeopleCounter.Service.Config;

    public abstract class MailHandler
    {
        private readonly ILogger<Worker> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public MailHandler(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected async Task ReadStreamAsync(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            using var reader = new StreamReader(stream);

            string[] lines = reader
                .ReadToEnd()
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var log = await GetLogAsync(line);
                if (log != null)
                {
                    await SaveLogsAsync(log);
                }
            }
        }

        protected async Task GetEmailAndSaveDataAsync(IEnumerable<BodyPartBasic> attachments, IMailFolder folder, UniqueId uid)
        {
            foreach (var attachment in attachments)
            {
                MimeEntity part = folder.GetBodyPart(uid, attachment);
                var fileName = part.ContentDisposition?.FileName ?? attachment.ContentType.Name;

                if (string.IsNullOrEmpty(fileName) ||
                    !fileName.EndsWith(FileExtensions.CSV, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var bytes = await GetBytesArrayToRead((MimePart)part);

                await ReadStreamAsync(bytes);
            }
        }

        private async Task<IList<Log>> GetLogAsync(string lineData)
        {
            IList<Log> logs = null;

            try
            {
                var data = lineData.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (data.Length > 5 && !data[0].Equals("date", StringComparison.InvariantCultureIgnoreCase))
                {
                    var date =
                        DateTime.ParseExact(
                            $"{data[0]} {data[1].Replace(".", string.Empty)}",
                            "dd/MM/yyyy hh:mm:ss tt",
                            CultureInfo.InvariantCulture);

                    int camId = 1;
                    logs = new List<Log>();

                    for (int i = 2; i < 26; i += 2)
                    {
                        if (await IsCamIdValid(camId))
                            logs.Add(await CreateLogAsync(camId, date, int.Parse(data[i]), int.Parse(data[i + 1])));

                        camId++;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Tried to convert a line with error, the line is [{lineData}]");
            }

            return logs;
        }

        private async Task<Log> CreateLogAsync(int camId, DateTime date, int enter, int exit)
        {
            return await Task.FromResult(
                new Log
                {
                    Date = date,
                    Entered = enter,
                    Exited = exit,
                    CameraId = camId
                });
        }

        private async Task SaveLogsAsync(IList<Log> logs)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GranEstacionContext>();

            await db.Logs.AddRangeAsync(logs);
            await db.SaveChangesAsync();
        }

        private async Task<bool> IsCamIdValid(int id)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GranEstacionContext>();

            var exists = db.Cameras.Any(cam => cam.CameraId == id);

            return await Task.FromResult(exists);
        }

        private async Task<byte[]> GetBytesArrayToRead(MimePart attachment)
        {
            using var stream = new MemoryStream();
            await attachment.Content.DecodeToAsync(stream);

            return stream.ToArray();
        }
    }
}
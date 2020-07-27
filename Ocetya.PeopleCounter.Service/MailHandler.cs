﻿namespace Ocetya.PeopleCounter.Service
{
    using Ocetya.PeopleCounter.Repository;
    using Ocetya.PeopleCounter.Service.Config;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MimeKit;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class MailHandler
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MailHandler(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected async Task GetEmailAndSaveDataAsync(MimeMessage message)
        {
            if (message.Attachments.ToArray().Length <= 0)
            {
                return;
            }

            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart part)
                {
                    var fileName = part.FileName;
                    if (!fileName.EndsWith(FileExtensions.CSV, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var bytes = await GetBytesArrayToRead(part);

                    using var stream = new MemoryStream(bytes);
                    using var reader = new StreamReader(stream);
                    string line = string.Empty;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var log = await GetLogAsync(line);
                        if (log != null)
                        {
                            await SaveLogsAsync(log);
                        }
                    }
                }
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
                _logger.LogError(ex, $"Tried to convert a line with error, the line is [{lineData}]");
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
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GranEstacionContext>();

            await db.Logs.AddRangeAsync(logs);
            await db.SaveChangesAsync();
        }

        private async Task<bool> IsCamIdValid(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
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
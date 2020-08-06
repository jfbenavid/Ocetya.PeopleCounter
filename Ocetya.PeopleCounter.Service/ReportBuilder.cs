namespace Ocetya.PeopleCounter.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using Ocetya.PeopleCounter.Repository;
    using Ocetya.PeopleCounter.Service.Config;
    using Ocetya.PeopleCounter.Service.Interfaces;
    using Ocetya.PeopleCounter.Service.Moldels;
    using Ocetya.PeopleCounter.Service.Moldels.CSVMaps;

    public class ReportBuilder : IReportBuilder
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOptions<MailConfiguration> _mailConfiguration;

        public ReportBuilder(IServiceScopeFactory serviceScopeFactory, IOptions<MailConfiguration> mailConfiguration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mailConfiguration = mailConfiguration;
        }

        private async Task<byte[]> BuildCSV<T, TMap>(IList<T> data) where TMap : ClassMap
        {
            using var memoryStream = new MemoryStream();
            using TextWriter streamWriter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TMap>();
            await csvWriter.WriteRecordsAsync(data);

            return await Task.FromResult(memoryStream.ToArray());
        }

        private async Task<List<DailyReport>> GetDailyReportAsync(DateTime beginDate)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GranEstacionContext>();

            return await db.Logs
                .Include(log => log.Camera)
                .AsNoTracking()
                .Where(log => log.Date >= beginDate && log.Date <= beginDate.AddDays(1))
                .Select(log => new DailyReport
                {
                    Date = log.Date,
                    Camera = log.Camera.Name,
                    Entered = log.Entered,
                    Exited = log.Exited
                })
                .ToListAsync();
        }

        public async Task<MimeMessage> BuildDailyReportAsync()
        {
            var data = await GetDailyReportAsync(DateTime.Today);

            return await GetMessageAsync<DailyReport, DailyReportMap>(
                "Reporte de logs diario",
                $"reporte-{DateTime.Today:dd-MM-yyyy}.csv",
                data);
        }

        public async Task<MimeMessage> BuildDayComparisonReport()
        {
            var todayReport = await GetDailyReportAsync(DateTime.Today);
            var lastYearReport = await GetDailyReportAsync(DateTime.Today.AddYears(-1));

            var data = todayReport
                .Join(
                    lastYearReport,
                    today => today.Date,
                    lastYear => lastYear.Date.AddYears(1),
                    (today, last) => new { today, last })
                .Select(joined => new ComparisonReport
                {
                    Camera = joined.today.Camera,
                    Date = joined.today.Date,
                    Entered = joined.today.Entered,
                    Exited = joined.today.Exited,
                    LastEntered = joined.last.Entered,
                    LastExited = joined.last.Exited
                })
                .ToList();

            return await GetMessageAsync<ComparisonReport, ComparisonReportMap>(
                "Reporte de logs diario",
                $"Comparacion_{DateTime.Today:dd-MM-yyyy}_{DateTime.Today.AddYears(-1):dd-MM-yyyy}.csv",
                data);
        }

        private async Task<MimeMessage> GetMessageAsync<T, TMap>(string subject, string fileName, IList<T> data) where TMap : ClassMap
        {
            var builder = new BodyBuilder
            {
                TextBody = string.Empty
            };

            builder.Attachments
                .Add(fileName, await BuildCSV<T, TMap>(data));

            var message = new MimeMessage
            {
                Body = builder.ToMessageBody(),
                Sender = new MailboxAddress(_mailConfiguration.Value.SmtpUser, _mailConfiguration.Value.SmtpUser),
                Subject = subject,
            };

            foreach (var adress in _mailConfiguration.Value.AdressesToSend)
            {
                message.To.Add(new MailboxAddress(adress, adress));
            }

            return message;
        }
    }
}
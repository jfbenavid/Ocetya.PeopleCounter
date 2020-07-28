namespace Ocetya.PeopleCounter.Service.Config
{
    using System.Collections.Generic;

    public class MailConfiguration
    {
        public int ImapPort { get; set; }

        public string ImapHost { get; set; }

        public string EmailsFrom { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPassword { get; set; }

        public string ImapUser { get; set; }

        public string ImapPassword { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public List<string> AdressesToSend { get; set; }
    }
}
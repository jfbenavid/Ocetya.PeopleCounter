namespace GranEstacion.Service.Config
{
    using System.Collections.Generic;

    public class MailConfiguration
    {
        public int ImapPort { get; set; }

        public string ImapHost { get; set; }

        public string EmailsFrom { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public List<string> AdressesToSend { get; set; }
    }
}
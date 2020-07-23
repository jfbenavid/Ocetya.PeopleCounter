namespace GranEstacion.Service.Models
{
    using MailKit;
    using System.Collections.Generic;

    public class ImapMessage
    {
        public IList<BodyPartBasic> Attachments { get; set; }
    }
}
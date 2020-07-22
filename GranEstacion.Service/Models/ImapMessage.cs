namespace GranEstacion.Service.Models
{
    using MailKit;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ImapMessage
    {
        public IList<BodyPartBasic> Attachments { get; set; }
    }
}
namespace GranEstacion.Service.Models
{
    using OpenPop.Mime;
    using System.Collections.Generic;

    public class POPMessage
    {
        public string MessageID;
        public string FromID;
        public string FromName;
        public string Subject;
        public string Body;
        public string Html;
        public string FileName;
        public IList<MessagePart> Attachment;
    }
}
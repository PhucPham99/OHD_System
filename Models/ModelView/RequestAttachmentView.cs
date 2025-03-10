using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.ModelView
{
    public class RequestAttachmentView
    {
        public int AttachmentID { get; set; }
        public int RequestID { get; set; }
        public string Requestname { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; } = 1;
    }
}
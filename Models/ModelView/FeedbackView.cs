using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.ModelView
{
    public class FeedbackView
    {
        public int FeedbackID { get; set; }
        public int RequestID { get; set; }
        public string RequestName {  get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; } = 1;

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.ModelView
{
    public class RequestResponView
    {
        public int ResponID { get; set; }
        public int RequestID { get; set; }
        public string RequestName { get; set; }
        public string ResponseText { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
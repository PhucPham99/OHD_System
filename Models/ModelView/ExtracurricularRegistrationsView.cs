using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.ModelView
{
    public class ExtracurricularRegistrationsView
    {
        public int RegistrationID { get; set; }
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Status { get; set; } = 1;
    }
}
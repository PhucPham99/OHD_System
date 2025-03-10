using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.ModelView
{
    public class RegistrationView
    {
        public int RegistrationID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Status { get; set; } = 1;

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.ModelView
{
    public class RequestView
    {
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } = 1;
        public int AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
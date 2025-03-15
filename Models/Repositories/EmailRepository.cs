using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using OHD_System.Models.Entities;

namespace OHD_System.Models.Repositories
{
    public class EmailRepository
    {
        private static EmailRepository instance;
        private EmailRepository() { }
        public static EmailRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmailRepository();
                }
                return instance;
            }
        }
        public string createEmail(Email newEmail)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.Emails.Add(newEmail);
                    en.SaveChanges();
                    return "create success";
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "create failed";
        }
       
    }
}
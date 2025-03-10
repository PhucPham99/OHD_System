using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using OHD_System.Models.Entities;

namespace OHD_System.Models.Repositories
{
    public class RoleRepository
    {
        private static RoleRepository instance;
        private RoleRepository() { }
        public static RoleRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleRepository();
                }
                return instance;
            }
        }
        public List<Role> getAllRole()
        {
            var ls = new List<Role>();
            try
            {
                using(var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Roles.ToList();
                    if (rs.Count > 0)
                    {
                        ls = rs;
                        return ls;
                    }
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ls;
        }
    }
}
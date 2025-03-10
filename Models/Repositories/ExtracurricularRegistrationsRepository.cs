using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using OHD_System.Models.Entities;
using OHD_System.Models.ModelView;

namespace OHD_System.Models.Repositories
{
    public class ExtracurricularRegistrationsRepository
    {
        private static ExtracurricularRegistrationsRepository _instance;
        private ExtracurricularRegistrationsRepository() { }
        public static ExtracurricularRegistrationsRepository instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExtracurricularRegistrationsRepository();
                }
                return _instance;
            }
        }
        public string CreateExtracurricularRegistration(Request newRequest)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.Requests.Add(newRequest);
                    en.SaveChanges();
                    return "create success ";
                }
            }
            catch (EntityException ex)
            {
                Debug.Write(ex.Message);
            }
            return "create failed";
        }
        public string DeleteExtracurricularRegistration(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.ExtracurricularRegistrations.Where(d => d.RegistrationID == id).FirstOrDefault();
                    en.ExtracurricularRegistrations.Remove(rs);
                    en.SaveChanges();
                    return "Delete success";
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "Delete failed";
        }
        public List<ExtracurricularRegistrationsView> GetExtracurricularRegistrations()
        {
            var ls = new List<ExtracurricularRegistrationsView>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = (from e in en.ExtracurricularRegistrations
                              from a in en.ExtracurricularActivities
                              from u in en.Users
                              where e.UserID == u.UserID && e.ActivityID == a.ActivityID    
                              select new ExtracurricularRegistrationsView
                              {
                                  RegistrationID=e.RegistrationID,
                                  ActivityID=e.ActivityID,
                                  ActivityName=a.Title,
                                  UserID=e.UserID,
                                  UserName=u.FullName,
                                  RegistrationDate=e.RegistrationDate??DateTime.MinValue,
                                  Status=e.Status??0
                              }).ToList();
                    if (rs.Count > 1)
                    {
                        ls = rs;
                    }
                    return ls;
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return ls;
        }
        public List<ExtracurricularRegistration> getAllExtraRegistration(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.ExtracurricularRegistrations.Count();
            return en.ExtracurricularRegistrations.OrderBy(p => p.RegistrationID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<ExtracurricularRegistrationsView> SearchExtraRegistrations(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = (from e in en.ExtracurricularRegistrations
                         from a in en.ExtracurricularActivities
                         from u in en.Users
                         select new ExtracurricularRegistrationsView
                         {
                             RegistrationID = e.RegistrationID,
                             ActivityID = e.ActivityID,
                             ActivityName = a.Title,
                             UserID = e.UserID,
                             UserName = u.FullName,
                             RegistrationDate = e.RegistrationDate ?? DateTime.MinValue,
                             Status = e.Status ?? 0
                         });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(rv => rv.ActivityName.Contains(searchTerm) || rv.UserName.Contains(searchTerm));
            }
            totalRecords = query.Count();
            return query.OrderBy(p => p.RegistrationID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
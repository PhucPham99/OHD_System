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
    public class RegistrationRepository
    {
        private static RegistrationRepository Instance;
        private RegistrationRepository() { }
        public static RegistrationRepository instance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new RegistrationRepository();
                }
                return Instance;
            }
        }
        public string CreateRegistration(Registration newRegistration)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.Registrations.Add(newRegistration);
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
        public string DeleteRegistration(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Registrations.Where(d => d.RegistrationID == id).FirstOrDefault();
                    en.Registrations.Remove(rs);
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
        public List<Registration> GetRegistrationByID(int id)
        {
            var ls = new List<Registration>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Registrations.Where(d => d.RegistrationID == id).ToList();
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
        public List<RegistrationView> GetRegistrations()
        {
            var ls = new List<RegistrationView>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = (from u in en.Users
                              from rp in en.Registrations
                              from c in en.Courses
                              where rp.UserID == u.UserID && rp.CourseID == c.CourseID
                              select new RegistrationView
                              {
                                  UserID = u.UserID,
                                  CourseID = c.CourseID,
                                  CourseName = c.CourseName,
                                  UserName = u.FullName,
                                  RegistrationID = rp.RegistrationID,
                                  RegistrationDate = rp.RegistrationDate ?? DateTime.MinValue,
                                  Status = rp.Status ?? 0,
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
        public List<Registration> getAllRegistration(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.Registrations.Count();
            return en.Registrations.OrderBy(p => p.RegistrationID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<RegistrationView> SearchRegistrations(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = (from u in en.Users
                         from rp in en.Registrations
                         from c in en.Courses
                         select new RegistrationView
                         {
                             RegistrationID = rp.RegistrationID,
                             CourseID = rp.CourseID,
                             CourseName = c.CourseName, // Lấy CourseName từ bảng Courses
                             UserID = rp.UserID,
                             UserName = u.FullName,
                             RegistrationDate = rp.RegistrationDate ?? DateTime.MinValue,
                         });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(rv => rv.CourseName.Contains(searchTerm) || rv.UserName.Contains(searchTerm));
            }
            totalRecords = query.Count();
            return query.OrderBy(p => p.RegistrationID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
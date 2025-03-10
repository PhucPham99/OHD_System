using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using OHD_System.Models.Entities;

namespace OHD_System.Models.Repositories
{
    public class ExtracurricularActivitiesRepository
    {
        private static ExtracurricularActivitiesRepository _instance;
        private ExtracurricularActivitiesRepository() { }
        public static ExtracurricularActivitiesRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExtracurricularActivitiesRepository();
                }
                return _instance;
            }
        }
        public string CreateExtracurricularActivity(ExtracurricularActivity newEA)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.ExtracurricularActivities.Add(newEA);
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
        public string DeleteExtracurricularActivity(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.ExtracurricularActivities.Where(d => d.ActivityID == id).FirstOrDefault();
                    en.ExtracurricularActivities.Remove(rs);
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
        public List<ExtracurricularActivity> GetActivities()
        {
            var ls = new List<ExtracurricularActivity>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.ExtracurricularActivities.ToList();
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
        public string updateActivity(int id, ExtracurricularActivity updateActivity)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.ExtracurricularActivities.Where(d => d.ActivityID == id).FirstOrDefault();
                    rs.Title = updateActivity.Title;
                    rs.Description = updateActivity.Description;    
                    rs.ActivityDate = updateActivity.ActivityDate;
                    rs.Location = updateActivity.Location;
                    rs.Capacity = updateActivity.Capacity;
                    rs.CreatedAt = updateActivity.CreatedAt;    
                    rs.Status = updateActivity.Status;
                    en.SaveChanges();
                    return "Update success";
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "update failed";
        }
        public List<ExtracurricularActivity> getAllActivity(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.ExtracurricularActivities.Count();
            return en.ExtracurricularActivities.OrderBy(p => p.ActivityID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<ExtracurricularActivity> SearchActivities(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = en.ExtracurricularActivities.Where(p => p.Title.Contains(searchTerm) ||
                                      p.Description.Contains(searchTerm) ||
                                       p.Location.Contains(searchTerm)).ToList();
            totalRecords = query.Count();
            return query.OrderBy(p => p.ActivityID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
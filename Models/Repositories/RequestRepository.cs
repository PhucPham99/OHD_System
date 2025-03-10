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
    public class RequestRepository
    {
        private static RequestRepository instance;
        private RequestRepository() { }
        public static RequestRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequestRepository();
                }
                return instance;
            }
        }
        public string CreateRequest(Request newRequest)
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
        public string DeleteRequest(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Requests.Where(d => d.RequestID == id).FirstOrDefault();
                    en.Requests.Remove(rs);
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
        public List<RequestView> GetUsers()
        {
            var ls = new List<RequestView>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = (from r in en.Requests
                              from u in en.Users
                              from c in en.Categories
                              where r.UserID == u.UserID && r.CategoryID == c.CategoryID
                              select new RequestView
                              {
                                  RequestID = r.RequestID,
                                  UserID = r.UserID,
                                  UserName = u.FullName,
                                  CategoryID = r.CategoryID,
                                  CategoryName = c.CategoryName,
                                  Description = r.Description,
                                  Status = r.Status ?? 0,
                                  AssignedTo = r.AssignedTo ?? 0,
                                  AssignedToName = u.FullName,
                                  CreatedAt = r.CreatedAt ?? DateTime.MinValue,
                                  UpdatedAt = r.UpdatedAt ?? DateTime.MinValue,
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
        public string updateRequest(int id, Request updateRequest)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Requests.Where(d => d.RequestID == id).FirstOrDefault();
                    if (rs != null)
                    {
                        rs.UserID = updateRequest.UserID;
                        rs.CategoryID = updateRequest.CategoryID;
                        rs.Description = updateRequest.Description;
                        rs.Status = updateRequest.Status;
                        rs.AssignedTo = updateRequest.AssignedTo;
                        rs.CreatedAt = updateRequest.CreatedAt;
                        rs.UpdatedAt = updateRequest.UpdatedAt;
                        en.SaveChanges();
                        return "update success";
                    }
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "update failed";
        }
        public List<Request> getAllRequest(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.Requests.Count();
            return en.Requests.OrderBy(p => p.RequestID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<Request> SearchRequests(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = en.Requests.Where(p => p.Description.Contains(searchTerm)).ToList();
            totalRecords = query.Count();
            return query.OrderBy(p => p.RequestID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
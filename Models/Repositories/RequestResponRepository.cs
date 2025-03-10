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
    public class RequestResponRepository
    {
        private static RequestResponRepository instance;
        private RequestResponRepository() { }
        public static RequestResponRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequestResponRepository();
                }
                return instance;
            }
        }
        public string CreateRequestRespon(RequestRespon newRequestRespon)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.RequestRespons.Add(newRequestRespon);
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
        public string DeleteRequestRespon(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.RequestRespons.Where(d => d.ResponID == id).FirstOrDefault();
                    en.RequestRespons.Remove(rs);
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
        public List<RequestResponView> GetRequestRespons()
        {
            var ls = new List<RequestResponView>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = (from r in en.Requests
                              from rp in en.RequestRespons
                              where r.RequestID == rp.RequestID 
                              select new RequestResponView
                              {
                                  ResponID = rp.RequestID,
                                  RequestID = r.RequestID,
                                  RequestName=r.Description,
                                  ResponseDate=rp.ResponseDate??DateTime.MinValue,
                                  ResponseText=rp.ResponseText
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
        public string updateRequestRespon(int id, RequestRespon updateRequestRespon)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.RequestRespons.Where(d => d.ResponID == id).FirstOrDefault();
                    if (rs != null)
                    {
                        rs.RequestID = updateRequestRespon.RequestID;
                        rs.ResponseDate = updateRequestRespon.ResponseDate;
                        rs.ResponseText = updateRequestRespon.ResponseText;
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
        public List<RequestRespon> getAllRequestRespon(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.RequestRespons.Count();
            return en.RequestRespons.OrderBy(p => p.ResponID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<RequestRespon> SearchRequestRespons(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = en.RequestRespons.Where(p => p.ResponseText.Contains(searchTerm)).ToList();
            totalRecords = query.Count();
            return query.OrderBy(p => p.ResponID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
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
    public class FeedbackRepository
    {
        private static FeedbackRepository instance;
        private FeedbackRepository() { }
        public static FeedbackRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FeedbackRepository();
                }
                return instance;
            }
        }
        public string CreateFeedback(Feedback newFeedback)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.Feedbacks.Add(newFeedback);
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
        public string DeleteFeedback(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Feedbacks.Where(d => d.FeedbackID == id).FirstOrDefault();
                    en.Feedbacks.Remove(rs);
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
        public List<FeedbackView> GetFeedbacks()
        {
            var ls = new List<FeedbackView>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = (from r in en.Requests
                              from f in en.Feedbacks
                              from u in en.Users
                              where r.RequestID == f.RequestID&&f.UserID == u.UserID
                              select new FeedbackView
                              {
                                  FeedbackID=f.FeedbackID,
                                  RequestID=f.RequestID,
                                  UserID=u.UserID,
                                  RequestName=r.Description,
                                  UserName=u.FullName,
                                  Rating=f.Rating??0,
                                  Comment=f.Comment,
                                  CreatedAt=f.CreatedAt??DateTime.MinValue,
                                  Status=f.Status??0
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
        public string updateFeedback(int id, Feedback updateFeedback)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Feedbacks.Where(d => d.FeedbackID == id).FirstOrDefault();
                    if (rs != null)
                    {
                        rs.RequestID = updateFeedback.RequestID;
                        rs.UserID = updateFeedback.UserID;
                        rs.Rating = updateFeedback.Rating;
                        rs.Comment = updateFeedback.Comment;
                        rs.CreatedAt = updateFeedback.CreatedAt;
                        rs.Status = updateFeedback.Status;
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
        public List<Feedback> getAllFeedback(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.Feedbacks.Count();
            return en.Feedbacks.OrderBy(p => p.FeedbackID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<FeedbackView> SearchFeedbacks(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = (from r in en.Requests
                         from f in en.Feedbacks
                         from u in en.Users
                         where r.RequestID == f.RequestID && f.UserID == u.UserID
                         select new FeedbackView
                         {
                             FeedbackID = f.FeedbackID,
                             RequestID = f.RequestID,
                             UserID = u.UserID,
                             RequestName = r.Description,
                             UserName = u.FullName,
                             Rating = f.Rating ?? 0,
                             Comment = f.Comment,
                             CreatedAt = f.CreatedAt ?? DateTime.MinValue,
                             Status = f.Status ?? 0
                         });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(rv => rv.RequestName.Contains(searchTerm) || rv.UserName.Contains(searchTerm));
            }
            totalRecords = query.Count();
            return query.OrderBy(p => p.FeedbackID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
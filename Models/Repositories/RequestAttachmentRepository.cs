using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.Ajax.Utilities;
using OHD_System.Models.Entities;
using OHD_System.Models.ModelView;

namespace OHD_System.Models.Repositories
{
    public class RequestAttachmentRepository
    {
        private static RequestAttachmentRepository instance;
        private RequestAttachmentRepository() { }
        public static RequestAttachmentRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequestAttachmentRepository();
                }
                return instance;
            }
        }
        public string CreateRequestAttachment(RequestAttachment newAttachment, HttpPostedFileBase file_name, string location)
        {
            try
            {
                var status_save_file = false;
                if (file_name != null)
                {
                    try
                    {
                        file_name.SaveAs($"{location}\\{newAttachment.FileName}");
                        status_save_file = true;
                    }
                    catch (HttpException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                if (status_save_file)
                {
                    var en = new Entities.OHD_SystemEntities();
                    RequestAttachment item = new RequestAttachment()
                    {
                        RequestID = newAttachment.RequestID,
                        AttachmentID = newAttachment.AttachmentID,
                        FileName = newAttachment.FileName,
                        CreatedAt = newAttachment.CreatedAt,
                        Status = newAttachment.Status,
                    };
                    en.RequestAttachments.Add(item);
                    en.SaveChanges();
                }
                else
                {
                    var en = new Entities.OHD_SystemEntities();
                    RequestAttachment item = new RequestAttachment()
                    {
                        RequestID = newAttachment.RequestID,
                        AttachmentID = newAttachment.AttachmentID,
                        FileName = newAttachment.FileName,
                        CreatedAt = newAttachment.CreatedAt,
                        Status = newAttachment.Status,
                    };
                    en.RequestAttachments.Add(item);
                    en.SaveChanges();
                };
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "create failed";
        }
        public List<RequestAttachmentView> GetMembers()
        {
            var list = new List<RequestAttachmentView>();
            try
            {
                var en = new Entities.OHD_SystemEntities();
                var q = (from r in en.Requests
                         from ra in en.RequestAttachments
                         where r.RequestID == ra.RequestID
                         select new RequestAttachmentView
                         {
                             AttachmentID = ra.AttachmentID,
                             RequestID = ra.RequestID,
                             Requestname = r.Description,
                             FileName = ra.FileName,
                             CreatedAt = ra.CreatedAt ?? DateTime.MinValue,
                             Status = ra.Status ?? 0
                         }).ToList();
                if (q.Count > 0)
                {
                    list = q;
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return list;
        }
        public string DeleteRequestAttachment(int id, string location)
        {
            try
            {
                if (string.IsNullOrEmpty(location))
                {
                    return "location is invalid";
                }
                var en = new Entities.OHD_SystemEntities();
                var result = en.RequestAttachments.Where(d => d.AttachmentID == id).FirstOrDefault();
                if (result != null)
                {
                    en.RequestAttachments.Remove(result);
                    en.SaveChanges();
                    if (!string.IsNullOrEmpty(result.FileName))
                    {
                        try
                        {
                            string rootPath = HostingEnvironment.MapPath("~/content/Admin/uploads/");
                            string fullPath = Path.Combine(rootPath, location);
                            if (File.Exists(fullPath))
                            {
                                File.Delete(fullPath);
                                return "delete success";
                            }
                            else
                            {
                                return "file not found, member deleted";
                            }
                        }
                        catch (IOException ex)
                        {
                            Debug.WriteLine($"Error deleting file: {ex.Message}");
                            return "delete member success, but file delete failed";
                        }
                    }

                    return "delete success (no file to delete)";
                }
                else
                {
                    return "member not found";
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                return "delete failed due to database error";
            }
        }
        public string Update_RequestAttachment(RequestAttachment updateAttachment, HttpPostedFileBase file_name, string location)
        {
            try
            {
                var status_save_file = false;
                if (file_name != null)
                {
                    try
                    {
                        file_name.SaveAs($"{location}\\{updateAttachment.FileName}");
                        status_save_file = true;
                    }
                    catch (HttpException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                if (status_save_file)
                {
                    var en = new Entities.OHD_SystemEntities();
                    var result = en.RequestAttachments.Where(d => d.AttachmentID == updateAttachment.AttachmentID).FirstOrDefault();
                    result.RequestID = updateAttachment.RequestID;
                    result.FileName = updateAttachment.FileName;
                    result.CreatedAt = updateAttachment.CreatedAt;
                    result.Status = updateAttachment.Status;
                    en.SaveChanges();
                    return "update Success";
                }
                else
                {
                    var en = new Entities.OHD_SystemEntities();
                    var result = en.RequestAttachments.Where(d => d.AttachmentID == updateAttachment.AttachmentID).FirstOrDefault();
                    result.RequestID = updateAttachment.RequestID;
                    result.FileName = updateAttachment.FileName;
                    result.CreatedAt = updateAttachment.CreatedAt;
                    result.Status = updateAttachment.Status;
                    en.SaveChanges();
                    return "update Success";
                }
            }
            catch (EntityException ex)
            {
                Debug.Write(ex.Message);
                return "update failed";
            }
        }
        public RequestAttachment GetAttachmentByID(int id)
        {
            RequestAttachment mem = new RequestAttachment();
            try
            {
                var en = new Entities.OHD_SystemEntities();
                var result = en.RequestAttachments.Where(d => d.AttachmentID == id).FirstOrDefault();
                mem = result;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return mem;
        }
        public List<RequestAttachmentView> SearchAttachment(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            using (var en = new Entities.OHD_SystemEntities())
            {
                var query = (from r in en.Requests
                             from ra in en.RequestAttachments
                             where r.RequestID == ra.RequestID
                             select new RequestAttachmentView
                             {
                                 AttachmentID = ra.AttachmentID,
                                 RequestID = ra.RequestID,
                                 Requestname = r.Description,
                                 FileName = ra.FileName,
                                 CreatedAt = ra.CreatedAt ?? DateTime.MinValue,
                                 Status = ra.Status ?? 0
                             });

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(rv => rv.Requestname.Contains(searchTerm));
                }
                totalRecords = query.Count();
                return query.OrderBy(p => p.AttachmentID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}
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
    public class UserRepository
    {
        private static UserRepository instance;
        private UserRepository() { }
        public static UserRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserRepository();
                }
                return instance;
            }
        }
        public string CreateUser(User newUser)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.Users.Add(newUser);
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
        public string DeleteUser(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Users.Where(d => d.UserID == id).FirstOrDefault();
                    en.Users.Remove(rs);
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
        public List<User> GetUsers()
        {
            var ls = new List<User>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Users.ToList();
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
        public string updateUser(int id, User updateUser)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Users.Where(d => d.UserID == id).FirstOrDefault();
                    rs.FullName = updateUser.FullName;
                    rs.PhoneNumber = updateUser.PhoneNumber;
                    rs.Status = updateUser.Status;
                    rs.Emails = updateUser.Emails;
                    rs.CreatedAt = updateUser.CreatedAt;
                    rs.UpdatedAt = updateUser.UpdatedAt;
                    rs.RoleID = updateUser.RoleID;
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

        public List<UserView> getAllUser(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.Users.Count();
            return (from u in en.Users
                    from r in en.Roles
                    where u.RoleID == r.RoleID
                    select new UserView
                    {
                        UserID = u.UserID,
                        FullName = u.FullName,
                        Email = u.Email,
                        PasswordHash = u.PasswordHash,
                        PhoneNumber = u.PhoneNumber,
                        RoleID = u.RoleID,
                        RoleName = r.RoleName,
                        Status = u.Status ?? 0,
                        CreatedAt = u.CreatedAt ?? DateTime.MinValue,
                        UpdatedAt = u.UpdatedAt ?? DateTime.MinValue
                    }).OrderBy(p => p.UserID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<UserView> SearchUser(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = (from u in en.Users
                         from r in en.Roles
                         where (u.FullName.Contains(searchTerm) || u.Email.Contains(searchTerm) || u.PhoneNumber.Contains(searchTerm))&&u.RoleID==r.RoleID
                         select new UserView
                         {
                             UserID = u.UserID,
                             FullName = u.FullName,
                             Email = u.Email,
                             PasswordHash = u.PasswordHash,
                             PhoneNumber = u.PhoneNumber,
                             RoleID = u.RoleID,
                             RoleName = r.RoleName,
                             Status = u.Status ?? 0,
                             CreatedAt = u.CreatedAt ?? DateTime.MinValue,
                             UpdatedAt = u.UpdatedAt ?? DateTime.MinValue
                         }).ToList();
            totalRecords = query.Count();
            return query.OrderBy(p => p.UserID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<User> checkUser(string param)
        {
            var ls = new List<User>();
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Users.Where(d => d.FullName.Equals(param)||d.PhoneNumber.Equals(param)||d.Email.Equals(param)).ToList();
                    if (rs.Count() > 0)
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
        public User getUserByID(int id)
        {
            var us = new User();
            try
            {
                using(var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Users.Where(u => u.UserID == id).FirstOrDefault();
                    if (rs != null)
                    {
                        us = rs;
                        return us;
                    }
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return us;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Web;
using OHD_System.Models.Entities;

namespace OHD_System.Models.Repositories
{
    public class CourseRepository
    {
        private static CourseRepository _instance;
        private CourseRepository() { }
        public static CourseRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CourseRepository();
                }
                return _instance;
            }
        }
        public string addCourse(Cours newCourse)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    en.Courses.Add(newCourse);
                    en.SaveChanges();
                    return " add success";
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "add failed ";
        }
        public string deleteCourse(int id)
        {
            try
            {
                using (var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Courses.Where(d => d.CourseID == id).FirstOrDefault();
                    if (rs != null)
                    {
                        en.Courses.Remove(rs);
                        en.SaveChanges();
                        return "delete success";
                    }
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return "delete failed";
        }
        public string updateCourse(Cours updateCourse,int id)
        {
            try
            {
                using(var en = new Entities.OHD_SystemEntities())
                {
                    var rs = en.Courses.Where(d=>d.CourseID == id).FirstOrDefault();    
                    if(rs != null)
                    {
                        rs.CourseName = updateCourse.CourseName;
                        rs.Department=updateCourse.Department;
                        rs.Description = updateCourse.Description;
                        rs.Credits = updateCourse.Credits;
                        rs.CreatedAt = updateCourse.CreatedAt;
                        rs.endDateAt=updateCourse.endDateAt;
                        rs.Status = updateCourse.Status;
                        en.SaveChanges();
                        return "update success";
                    }
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return " update failed";
        }
        public List<Cours> getAllCourse(int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            totalRecords = en.Courses.Count();
            return en.Courses.OrderBy(p => p.CourseID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<Cours> SearchCourses(string searchTerm, int pageIndex, int pageSize, out int totalRecords)
        {
            var en = new Entities.OHD_SystemEntities();
            var query = en.Courses.Where(p => p.CourseName.Contains(searchTerm) ||
                                      p.Description.Contains(searchTerm) ||
                                       p.Department.Contains(searchTerm)).ToList();
            totalRecords = query.Count();
            return query.OrderBy(p => p.CourseID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
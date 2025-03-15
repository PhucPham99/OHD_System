using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using OHD_System.Models.Entities;

namespace OHD_System.Models.Utils
{
    public class MyValidate
    {
        private static MyValidate _instance;
        public static MyValidate Instance
        {
            get
            {
                if (_instance == null)
                { _instance = new MyValidate(); }
                return _instance;
            }
        }
        public string CheckFullName(string val)
        {

            if (val == null)
            {
                return "Please enter Name";
            }
            if (!Regex.IsMatch(val, RegexPatterns.FullName))
            {
                return "Invalid Name";
            }
            return "";
        }
        public string CheckPhoneNumber(string val)
        {
            if (val == null)
            {
                return "Please enter Phone";
            }
            if (!Regex.IsMatch(val, RegexPatterns.PhoneNumber))
            {
                return "Invalid Phone";
            }
            return "";
        }
        public string CheckEmail(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return "Please enter an Email.";
            }

            val = val.Trim(); // Xóa khoảng trắng đầu & cuối

            // Kiểm tra ký tự '@' có tồn tại không
            if (!val.Contains("@"))
            {
                return "Invalid Email: Missing '@' symbol.";
            }

            // Kiểm tra phần tên và phần domain có hợp lệ không
            string[] parts = val.Split('@');
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                return "Invalid Email: Incomplete email address.";
            }

            // Kiểm tra domain có dấu '.' không
            if (!parts[1].Contains("."))
            {
                return "Invalid Email: Missing domain extension (e.g., '.com').";
            }

            // Kiểm tra với regex chuẩn từ class RegexPatterns
            if (!Regex.IsMatch(val, RegexPatterns.Email))
            {
                return "Invalid Email: Contains invalid characters or incorrect format.";
            }
            return "";
        }
        public string CheckType(string val, string Name)
        {
            if (val == "0")
            {
                return "Please choose" + Name;
            }
            return "";
        }
        public string CheckSpecialization(string val)
        {
            if (val == null)
            {
                return "please enter Specialization";
            }
            if (!Regex.IsMatch(val, RegexPatterns.Specialization, RegexOptions.IgnoreCase))
            {
                return "The trainer must have expertise in specialized fields such as yoga, fitness, pilates, zumba, crossfit, or bodybuilding.";
            }
            return "";
        }
        public string CheckExperience(string val, string Name)
        {
            if (val == null)
            {
                return "Please enter " + Name;
            }
            if (!Regex.IsMatch(val, RegexPatterns.Experience))
            {
                return "Invalid ";
            }
            return "";
        }
        public string CheckDate(string val, string Name)
        {
            var today = DateTime.Now;
            var day = today.Day;
            if (int.Parse(val) > day)
            {
                return "invalid" + Name;
            }
            else
            {
                return "";
            }
        }
        public string CheckDoB(string val)
        {
            var today = DateTime.Now;
            var age = today.Year - int.Parse(val);
            if (age < 18)
            {
                return "users under 18 years old";
            }
            return "";
        }
        public string CheckNumbers(string val, string name)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter Credits";
            if (!Regex.IsMatch(val, RegexPatterns.Numbers))
                return name + "Must be a positive integer.";
            return "";
        }
        public string CheckDepartment(string val, string name)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter Department";
            if (!Regex.IsMatch(val, RegexPatterns.Department))
                return name + " Only letters and spaces are allowed.";
            return "";
        }
        public string CheckInput(string val,string name)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter "+name;
            return "";
        }
        public string CheckEndDateAt(string val, string createdAt)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter EndDateAt date";
            if (!DateTime.TryParse(val, out DateTime endDate))
                return "Invalid EndDateAt format";
            if (!DateTime.TryParse(createdAt, out DateTime startDate))
                return "Invalid CreatedAt format";
            if (endDate < startDate)
                return "EndDateAt cannot be before CreatedAt";
            return "";
        }
        public string ValidateCreatedAt(string input,string name)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please choose a date";
            }
            if (!DateTime.TryParse(input, out DateTime createdAt))
            {
                return "Invalid date format";
            }

            DateTime today = DateTime.Today;

            if (createdAt < today || createdAt > today)
            {
                return name+" must be today";
            }

            return "";
        }
        public string CheckCourseName(string val ,string name)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter Course Name";
            if (!Regex.IsMatch(val, RegexPatterns.CourseName))
                return "Invalid"+ name+". Only letters, numbers, and spaces are allowed.";
            return "";
        }
        public string CheckPassword(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter Password";
            if (!Regex.IsMatch(val, RegexPatterns.Password))
                return "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.";
            return "";
        }
        public string CheckIDUser(string val,string name)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "Please enter Name";
            if (!Regex.IsMatch(val, RegexPatterns.IDUser))
                return "Invalid Name. Only letters and spaces are allowed, and maximum length is 16 characters.";
            return "";
        }
        public string ValidateUpdatedAt(string input, string name)
        {
            if (!DateTime.TryParse(input, out DateTime updatedAt))
            {
                return "Invalid date format";
            }

            DateTime today = DateTime.Today;

            if (updatedAt < today)
            {
                return name + " cannot be in the past";
            }

            return "";
        }
    }
}
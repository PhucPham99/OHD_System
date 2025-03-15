using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OHD_System.Models.Utils
{
    public class RegexPatterns
    {
        public const string Specialization = @"\b(yoga|fitness|pilates|zumba|crossfit|bodybuilding)\b";
        public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string PhoneNumber = @"^(03|05|07|08|09)[0-9]{8}$";
        public const string FullName = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠăđĩũơẲẰẮẴẶĐ̀Đ́Đ̃Đ̣Đ̉Đ̣ỞỜỠỚỢỪỮỨỰỬỰỪừữứựÝỲỸỴỶýỳỹỵỷ\s]+$";
        public const string Experience = @"^[1-9]\d*$";
        public const string CourseName = @"^[a-zA-Z0-9\s\-]+$";
        public const string Numbers = @"^\d+$";
        public const string Department = @"^[a-zA-Z\s]+$";
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public const string IDUser = @"^[a-zA-Z\s]{1,16}$";

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    internal class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public Student(int id, string name)
        {
            StudentID = id;
            StudentName = name;
        }
    }
}

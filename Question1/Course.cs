using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    internal class Course
    {
        public int CourseID { get; set; }
        public string CourseTitle { get; set; }

        private Dictionary<Student, double> studentGPA;

        public delegate void StudentChange(int oldNum, int newNum);

        public event StudentChange OnNumberOfStudentChange;
       

        public Course(int id, string title)
        {
            CourseID = id;
            CourseTitle = title;
            studentGPA = new Dictionary<Student, double>();
        }

        public void AddStudent(Student d, double g)
        {
            int oldNum = studentGPA.Count;
            studentGPA.Add(d, g);
            int newNum = studentGPA.Count;
            if (OnNumberOfStudentChange != null)
            {
                OnNumberOfStudentChange(oldNum, newNum);
            }
            
        }

        public void RemoveStudent(int StudentID)
        {
            int oldNum = studentGPA.Count;
            // find student by id
            Student s = null;
            foreach (KeyValuePair<Student, double> kv in studentGPA) {
                if (kv.Key.StudentID == StudentID)
                {
                    s = kv.Key;
                }
            }
            // remove student
            studentGPA.Remove(s);
            int newNum = studentGPA.Count;
            if (OnNumberOfStudentChange != null)
            {
                OnNumberOfStudentChange(oldNum, newNum);
            }
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(string.Format("Course: {0} - {1}", CourseID, CourseTitle));
            foreach (Student d in studentGPA.Keys)
            {
                res.AppendLine(string.Format("Student: {0} - {1} - Mark: {2}", d.StudentID, d.StudentName, studentGPA[d]));
            }

            return res.ToString();
        }
    }
}

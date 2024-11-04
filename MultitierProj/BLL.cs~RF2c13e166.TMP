using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    internal class Programs
    {
        internal static int UpdatePrograms()
        {
             return Data.Programs.UpdatePrograms();
        }
        
    }

    internal class Courses
    {
        internal static int UpdateCourses()
        {
            DataTable dt = Data.Courses.GetCourses()
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            if ((dt != null) && (dt.Select("Prix < 10.0").Length > 0))
            {
                College1en.Form1.msgCommandTooLow();
                Data.Courses.GetCourses().RejectChanges();
                return -1;
            }
            else
            {
                return Data.Courses.UpdateCourses();
            }
        }

    }   

    internal class Enrollments
    {
        internal static int UpdateEnrollments()
        {

            DataTable dt = Data.Enrollments.GetEnrollments()
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            if ((dt != null) && (dt.Select("Prix < 10.0").Length > 0))
            {
                College1en.Form1.msgCommandTooLow();
                Data.Enrollments.GetEnrollments().RejectChanges();
                return -1;
            }
            else
            {
                return Data.Enrollments.UpdateEnrollments();
            }
           
        }

    }

    internal class Students
    {
        internal static int UpdateStudents()
        {
            DataTable dt = Data.Students.GetStudents()
                             .GetChanges(DataRowState.Added | DataRowState.Modified);
            if ((dt != null) && (dt.Select("Prix < 10.0").Length > 0))
            {
                College1en.Form1.msgCommandTooLow();
                Data.Students.GetStudents().RejectChanges();
                return -1;
            }
            else
            {
                return Data.Students.UpdateStudents();
            }


          
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessLayer
{
    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Programs"].GetChanges(DataRowState.Added | DataRowState.Modified);

            if( dt != null)
            {
                if(dt.AsEnumerable().Any(r => !IsValidPid(r.Field<string> ("ProgId"))))
                {
                    College1en.Form1.BLLMessage("Invalid Program Id");
                    ds.RejectChanges();
                    return -1;
                }
                else
                {
                    return Data.Programs.UpdatePrograms();
                }
            }
            else
            {
                return Data.Programs.UpdatePrograms();
            }
        }

        private static bool IsValidPid(string ProgId)
        {
            bool r = true;
            if (ProgId.Length != 5) { r = false; }
            else if (ProgId[0] != 'P') { r = false; }
            else
            {
                for (int i = 1; i < ProgId.Length; i++)
                {
                    r = r && Char.IsDigit(ProgId[i]);
                }
            }
            return r;
        }

    }

    internal class Courses
    {
        internal static int UpdateCourses()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Courses"].GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidCId(r.Field<string>("CId"))))
                //(!IsValidProjId(dt.Rows[0].Field<string>("ProjId")))
                {
                    College1en.Form1.BLLMessage("Invalid Id for Projects");
       
                    ds.RejectChanges();
                    return -1;
                }
                else
                {

                    return Data.Courses.UpdateCourses();
                }
            }
            else
            {

                return Data.Courses.UpdateCourses();
            }
        }



        private static bool IsValidCId(string CId)
        {
            bool r = true;
            if (CId.Length != 7) { r = false; }
            else if (CId[0] != 'C') { r = false; }
            else
            {
                for (int i = 1; i < CId.Length; i++)
                {
                    r = r && Char.IsDigit(CId[i]);
                }
            }
            return r;
        }

    }


    internal class Students
    {
        internal static int UpdateStudents()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Students"].GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidStId(r.Field<string>("StId"))))

                {
                    College1en.Form1.BLLMessage("Invalid Id for Projects");

                    ds.RejectChanges();
                    return -1;
                }
                else
                {
                    return Data.Students.UpdateStudents();
                }
            }
            else
            {
                return Data.Students.UpdateStudents();
            }
        }



        private static bool IsValidStId(string StId)
        {
            bool r = true;
            if (StId.Length != 10) { r = false; }
            else if (StId[0] != 'S') { r = false; }
            else
            {
                for (int i = 1; i < StId.Length; i++)
                {
                    r = r && Char.IsDigit(StId[i]);
                }
            }
            return r;
        }

    }

    internal class Enrollments
    {
        internal static int UpdateFinalGrade(string[] a, string ev)
        {
            Nullable<int> eval;
            int temp;

            if (ev == "")
            {
                eval = null;
            }
            else if (int.TryParse(ev, out temp) && (0 <= temp && temp <= 100))
            {
                eval = temp;
               
            }
            else if (ev.Any(c => !Char.IsDigit(c))) 
            {
                
                College1en.Form1.BLLMessage("Final Grade must be a valid integer between 0 and 100  or null.");
                return -1;
            }
            else
            {
                College1en.Form1.BLLMessage(
                          "Final Grade must be an integer between 0 and 100"
                          );
                return -1;
            }

            return Data.Enrollments.UpdateFinal(a, eval);

           

            //DataTable dt = Data.Enrollments.GetEnrollments()
            //                  .GetChanges(DataRowState.Added | DataRowState.Modified);


        }

    }

    
}

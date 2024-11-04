using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();

        internal static String ConnectionString { get => cliComConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }


    internal class DataTables
    {
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterEnrollments = InitAdapterEnrollments();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgId",
                Connect.ConnectionString
                );

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();
          

            return r;
        }

        private static SqlDataAdapter InitAdapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Courses ORDER BY CId",
                Connect.ConnectionString
                );

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }


        private static SqlDataAdapter InitAdapterEnrollments()
        {
           SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY CId",
                Connect.ConnectionString
                );

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }


        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId",
                Connect.ConnectionString
                );

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;

        }


        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadCourses(ds);
            loadStudents(ds);
            loadEnrollments(ds);
           
            return ds;
        }

        private static void loadPrograms(DataSet ds)
        {
            adapterPrograms.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterPrograms.Fill(ds, "Programs");

        }

        private static void loadStudents(DataSet ds)
        {
            adapterStudents.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterStudents.Fill(ds, "Students");

           
          
                ForeignKeyConstraint fkc = new ForeignKeyConstraint(
                    "FK_Students_Programs",
                    ds.Tables["Students"].Columns["ProgId"],
                    ds.Tables["Programs"].Columns["ProgId"]
                );
                ds.Tables["Programs"].Constraints.Add(fkc);
                fkc.DeleteRule = Rule.Cascade;
                fkc.UpdateRule = Rule.Cascade;
            
            
            
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterCourses.Fill(ds, "Courses");

          
                ForeignKeyConstraint fkc = new ForeignKeyConstraint(
                    "FK_Courses_Programs",
                    ds.Tables["Programs"].Columns["ProgId"],
                    ds.Tables["Courses"].Columns["ProgId"]
                );

                ds.Tables["Courses"].Constraints.Add(fkc);
                fkc.DeleteRule = Rule.Cascade;
                fkc.UpdateRule = Rule.Cascade;
            
        }

        private static void loadEnrollments(DataSet ds)
        {
            adapterEnrollments.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterEnrollments.Fill(ds, "Enrollments");

         
                ForeignKeyConstraint fkc = new ForeignKeyConstraint(
                    "FK_Enrollments_Courses",
                    ds.Tables["Courses"].Columns["CId"],  
                    ds.Tables["Enrollments"].Columns["CId"]  
                );

                ds.Tables["Enrollments"].Constraints.Add(fkc);
                fkc.DeleteRule = Rule.Cascade;
                fkc.UpdateRule = Rule.Cascade;
           
          
                ForeignKeyConstraint fkc2 = new ForeignKeyConstraint(
                    "FK_Enrollments_Students",
                    ds.Tables["Students"].Columns["StId"],  
                    ds.Tables["Enrollments"].Columns["StId"]  
                );

                ds.Tables["Enrollments"].Constraints.Add(fkc2);
                fkc2.DeleteRule = Rule.Cascade;
                fkc2.UpdateRule = Rule.Cascade;
          
        }









        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        

        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }


        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }


        internal static SqlDataAdapter getAdapterEnrollments()
        {
            return adapterEnrollments;
        }


        internal static DataSet getDataSet()
        {
            return ds;
        }

    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        { 
            return ds.Tables["Programs"];
        }


        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors) 
            { 
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }


    }


    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
        private static DataSet ds = DataTables.getDataSet();


        internal static DataTable GetCourses()
        {
            return ds.Tables["Courses"];
        }


        internal static int UpdateCourses()
        {
            if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }





    }


    internal class Enrollments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollments();
        private static DataSet ds = DataTables.getDataSet();


        internal static DataTable GetEnrollments()
        {
            return ds.Tables["Enrollments"];
        }


        internal static int UpdateEnrollments()
        {
            if (!ds.Tables["Enrollments"].HasErrors)
            {
                return adapter.Update(ds.Tables["Enrollments"]);
            }
            else
            {
                return -1;
            }
        }
    }



    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds = DataTables.getDataSet();


        internal static DataTable GetStudents()
        {
            return ds.Tables["Students"];
        }


        internal static int UpdateStudents()
        {
            if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }











}

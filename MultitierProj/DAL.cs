﻿using System;
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
                "SELECT * FROM Enrollments ORDER BY StId, CId",
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
            //adapterPrograms.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterPrograms.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1] { ds.Tables["Programs"].Columns["ProgId"] };

        }
        private static void loadCourses(DataSet ds)
        {
            //adapterCourses.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterCourses.Fill(ds, "Courses");


            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1] { ds.Tables["Courses"].Columns["CId"] };
          
                ForeignKeyConstraint fkc = new ForeignKeyConstraint(
                    "FK_Courses_Programs",
                    ds.Tables["Programs"].Columns["ProgId"],
                    ds.Tables["Courses"].Columns["ProgId"]
                );

                ds.Tables["Courses"].Constraints.Add(fkc);
                fkc.DeleteRule = Rule.Cascade;
                fkc.UpdateRule = Rule.Cascade;
            
        }

        private static void loadStudents(DataSet ds)
        {
            //adapterStudents.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterStudents.Fill(ds, "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1] { ds.Tables["Students"].Columns["StId"] };



            ForeignKeyConstraint fkc = new ForeignKeyConstraint(
                "FK_Students_Programs",
                 ds.Tables["Programs"].Columns["ProgId"],
                ds.Tables["Students"].Columns["ProgId"]
               
            );
            ds.Tables["Students"].Constraints.Add(fkc);
            fkc.DeleteRule = Rule.None;
            fkc.UpdateRule = Rule.Cascade;



        }


        private static void loadEnrollments(DataSet ds)
        {
            //adapterEnrollments.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterEnrollments.Fill(ds, "Enrollments");

            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2] { ds.Tables["Enrollments"].Columns["StId"], ds.Tables["Enrollments"].Columns["CId"] };

         
                ForeignKeyConstraint fkc = new ForeignKeyConstraint(
                    "FK_Enrollments_Courses",
                    ds.Tables["Courses"].Columns["CId"],  
                    ds.Tables["Enrollments"].Columns["CId"]  
                );

                ds.Tables["Enrollments"].Constraints.Add(fkc);
                fkc.DeleteRule = Rule.None;
                fkc.UpdateRule = Rule.None;
           
          
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
            try
            {
                var a = new string[2]; // Assuming a[0] is the new program ID and a[1] is the course ID being checked.

                // Retrieve the current program ID for the course from the "Courses" table.
                var existingProgramId = (
                    from course in ds.Tables["Courses"].AsEnumerable()
                    where course.Field<string>("CId") == a[1]
                    select course.Field<string>("ProgId")
                ).FirstOrDefault();

                // Check if the course is already assigned to a different program.
                if (existingProgramId != null && existingProgramId != a[0])
                {
                    College1en.Form1.BLLMessage("Error: This course is already associated with a different program.");
                    return -1;
                }

                if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
               
                return -1;
            }
            }
            catch
            {
                College1en.Form1.BLLMessage("Error: The course already exists.");
                return -1;
            }
        }





    }

    internal class Enrollments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollments();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayEnrollments = null;

        internal static DataTable GetEnrollments()
        {

            ds.Tables["Enrollments"].AcceptChanges();

            var query = (
            from enroll in ds.Tables["Enrollments"].AsEnumerable()
            from prog in ds.Tables["Programs"].AsEnumerable()
            from course in ds.Tables["Courses"].AsEnumerable()
            from student in ds.Tables["Students"].AsEnumerable()
            where prog.Field<string>("ProgId") == course.Field<string>("ProgId")
            where enroll.Field<string>("CId") == course.Field<string>("CId")
            where enroll.Field<string>("StId") == student.Field<string>("StId")
         
            

          
            select new
            {
                StId = student.Field<string>("StId"),
                StName = student.Field<string>("StName"),
                CId = course.Field<string>("CId"),
                CName = course.Field<string>("CName"),
                FinalGrade = enroll.Field<Nullable<int>>("FinalGrade"),
                ProgId = prog.Field<string>("ProgId"),
                ProgName = prog.Field<string>("ProgName")

            });

            DataTable result = new DataTable();
            result.Columns.Add("StId");
            result.Columns.Add("StName");
            result.Columns.Add("CId");
            result.Columns.Add("CName");
            result.Columns.Add("FinalGrade");
            result.Columns.Add("ProgId");
            result.Columns.Add("ProgName");
            foreach (var item in query)
            {
                object[] allFields = { item.StId,item.StName, item.CId,item.CName, item.FinalGrade, item.ProgId, item.ProgName };
                result.Rows.Add(allFields);
            }
            displayEnrollments = result;
            return displayEnrollments;
        }


        internal static int AddData(string[] a)
        {
          
            var test = (
                   from enroll in ds.Tables["Enrollments"].AsEnumerable()
                   where enroll.Field<string>("StId") == a[0]
                   where enroll.Field<string>("CId") == a[1]
                   select enroll);
            if (test.Count() > 0)
            {
                College1en.Form1.DALMessage("This assignment already exists");
                return -1;
            }
            var test2 = (
                from courses in ds.Tables["Courses"].AsEnumerable()
                from students in ds.Tables["Students"].AsEnumerable()
                where courses.Field<string>("CId") == a[1]
                where students.Field<string>("StId") == a[0]
                where students.Field<string>("ProgId") == courses.Field<string>("ProgId")
                select courses);
            if (test2.Count() == 0)
            {
                College1en.Form1.BLLMessage("Error: The student cannot enroll in this course as it is not part of their program.");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();
                line.SetField("StId", a[0]);
                line.SetField("CId", a[1]);
                ds.Tables["Enrollments"].Rows.Add(line);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollments != null)
                {
                    var query = (
                           from std in ds.Tables["Students"].AsEnumerable()
                           from cid in ds.Tables["Courses"].AsEnumerable()
                           from pid in ds.Tables["Programs"].AsEnumerable()
                           where std.Field<string>("StId") == a[0]
                           where cid.Field<string>("CId") == a[1]
                         
                           select new
                           {
                               StId = std.Field<string>("StId"),
                               StName = std.Field<string>("StName"),
                               CId = cid.Field<string>("CId"),
                               CName = cid.Field<string>("CName"),
                               ProgId = pid.Field<string>("ProgId"),
                               ProgName = pid.Field<string>("ProgName"),
                               Fgrade = line.Field<Nullable<int>>("FinalGrade")
                           });
                
                    var r = query.First();
                    displayEnrollments.Rows.Add(new object[] { r.StId, r.StName, r.CId, r.CName, r.Fgrade, r.ProgId, r.ProgName });
                    College1en.Form1.DALMessage("Insertion successful");
                }
                return 0;
        }
            catch (Exception)
            {
                College1en.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
}

        internal static int ModData(string[] a)
        {
           // var studentProgramId = (
           //from std in ds.Tables["Students"].AsEnumerable()
           //where std.Field<string>("StId") == a[0]
           //select std.Field<string>("ProgId")
           //    ).FirstOrDefault();

           // var courseProgramId = (
           //     from cid in ds.Tables["Courses"].AsEnumerable()
           //     where cid.Field<string>("CId") == a[1]
           //     select cid.Field<string>("ProgId")
           // ).FirstOrDefault();

           // if (studentProgramId != courseProgramId)
           // {
           //     College1en.Form1.BLLMessage("Error: The student cannot enroll in this course as it is not part of their program.");
           //     return -1;
           // }

           // var test = (
           //        from enroll in ds.Tables["Enrollments"].AsEnumerable()
           //        where enroll.Field<string>("StId") == a[0]
           //        where enroll.Field<string>("CId") == a[1]
           //        select enroll);
           // if (test.Count() > 0)
           // {
           //     College1en.Form1.DALMessage("This assignment already exists");
           //     return -1;
           // }
           // try
           // {
           //     DataRow line = ds.Tables["Enrollments"].NewRow();
           //     line.SetField("StId", a[0]);
           //     line.SetField("CId", a[1]);
           //     ds.Tables["Enrollments"].Rows.Add(line);

           //     adapter.Update(ds.Tables["Enrollments"]);

           //     if (displayEnrollments != null)
           //     {
           //         var query = (
           //                from std in ds.Tables["Students"].AsEnumerable()
           //                from cid in ds.Tables["Courses"].AsEnumerable()
           //                where std.Field<string>("StId") == a[0]
           //                where cid.Field<string>("CId") == a[1]
           //                select new
           //                {
           //                    StId = std.Field<string>("StId"),
           //                    StName = std.Field<string>("StName"),
           //                    CId = cid.Field<string>("CId"),
           //                    CName = cid.Field<string>("CName"),
           //                    //Fgrade = line.Field<Nullable<int>>("FinalGrade")
           //                });

           //         var r = query.Single();
           //         displayEnrollments.Rows.Add(new object[] { r.StId, r.StName, r.CId, r.CName/*, r.Fgrade */});
           //         College1en.Form1.DALMessage("Modication successful");
           //     }
           //     return 0;
           // }
           // catch (Exception)
           // {
           //     College1en.Form1.DALMessage("Insertion / Update rejected");
           //     return -1;
           // }
           return 0;
        }


        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                var lines = ds.Tables["Enrollments"].AsEnumerable()
                                .Where(s =>
                                   lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                
                foreach (var line in lines)
                {
                    if (line.Field<Nullable<int>>("FinalGrade") != null)
                    {
                        College1en.Form1.BLLMessage("Enrollment cannot be deleted, Final Grade must be null");
                        return -1;
                    }
                }

                foreach (var line in lines)
                {
                    line.Delete();
                }

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollments != null )
                {
                    foreach (var p in lId)
                    {
                        var r = displayEnrollments.AsEnumerable()
                                .Where(s => (s.Field<string>("StId") == p[0] && s.Field<string>("CId") == p[1]))
                                .Single();
                        displayEnrollments.Rows.Remove(r);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                College1en.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }


        internal static int UpdateFinal(string[] a, Nullable<int> eval)
        {
            try
            {
                
                var line = ds.Tables["Enrollments"].AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();
           

                line.SetField("FinalGrade", eval);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollments != null)
                {
                    var r = displayEnrollments.AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .SingleOrDefault();
                    r.SetField("FinalGrade", eval);
                }
                return 0;
            }
            catch (Exception)
            {
                College1en.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }


        
    }













}

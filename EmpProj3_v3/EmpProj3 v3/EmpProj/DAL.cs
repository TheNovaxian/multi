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
            cs.InitialCatalog = "EmpProj3";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterEmp = InitAdapterEmp();
        private static SqlDataAdapter adapterProj = InitAdapterProj();
        private static SqlDataAdapter adapterAssign = InitAdapterAssign();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterEmp()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Employees ORDER BY EmpId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterProj()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Projects ORDER BY ProjId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterAssign()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Assignments ORDER BY EmpId, ProjId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();          
            loadEmp(ds);          
            loadProj(ds);           
            loadAssign(ds);          
            return ds;
        }

        private static void loadEmp(DataSet ds)
        {
            // =========================================================================
            //adapterEmp.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            // =========================================================================

            adapterEmp.Fill(ds, "Employees");

            // =========================================================================
            ds.Tables["Employees"].Columns["EmpId"].AllowDBNull = false;
            ds.Tables["Employees"].Columns["EmpName"].AllowDBNull = false;

            ds.Tables["Employees"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Employees"].Columns["EmpId"]};
            // =========================================================================
        }

        private static void loadProj(DataSet ds)
        {
            adapterProj.Fill(ds, "Projects");

            // =========================================================================
            ds.Tables["Projects"].Columns["ProjId"].AllowDBNull = false;
            ds.Tables["Projects"].Columns["ProjName"].AllowDBNull = false;
           
            ds.Tables["Projects"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Projects"].Columns["ProjId"]};
            // =========================================================================    
        }

        private static void loadAssign(DataSet ds)
        {
            adapterAssign.Fill(ds, "Assignments");

            // =========================================================================
            ds.Tables["Assignments"].Columns["EmpId"].AllowDBNull = false;
            ds.Tables["Assignments"].Columns["ProjId"].AllowDBNull = false;

            ds.Tables["Assignments"].PrimaryKey = new DataColumn[2]
                    { ds.Tables["Assignments"].Columns["EmpId"], ds.Tables["Assignments"].Columns["ProjId"] };

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK01 = new ForeignKeyConstraint("MyFK01",
                new DataColumn[]{
                    ds.Tables["Employees"].Columns["EmpId"]
                },
                new DataColumn[] {
                    ds.Tables["Assignments"].Columns["EmpId"]
                }
            );
            myFK01.DeleteRule = Rule.Cascade;
            myFK01.UpdateRule = Rule.Cascade;
            ds.Tables["Assignments"].Constraints.Add(myFK01);

            ForeignKeyConstraint myFK02 = new ForeignKeyConstraint("MyFK02",
              new DataColumn[]{
                    ds.Tables["Projects"].Columns["ProjId"]
              },
              new DataColumn[] {
                    ds.Tables["Assignments"].Columns["ProjId"]
              }
          );
            myFK02.DeleteRule = Rule.None;
            myFK02.UpdateRule = Rule.Cascade;
            ds.Tables["Assignments"].Constraints.Add(myFK02);

            // =========================================================================  
        }

        internal static SqlDataAdapter getAdapterEmp()
        {
            return adapterEmp;
        }
        internal static SqlDataAdapter getAdapterProj()
        {
            return adapterProj;
        }
        internal static SqlDataAdapter getAdapterAssign()
        {
            return adapterAssign;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
    }

    internal class Employees
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEmp();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetEmployees()
        {
            return ds.Tables["Employees"];
        }

        internal static int UpdateEmployees()
        {
            if (!ds.Tables["Employees"].HasErrors)
            {
                return adapter.Update(ds.Tables["Employees"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Projects
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterProj();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetProjects()
        {
            return ds.Tables["Projects"];
        }

        internal static int UpdateProjects()
        {
            if (!ds.Tables["Projects"].HasErrors)
            {
                return adapter.Update(ds.Tables["Projects"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Assignments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterAssign();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayAssign = null;

        internal static DataTable GetDisplayAssignments()
        {
            /* 
             * next line is needed to ensure "delete row"
             * due to the cascade are actually removed.
             */
            ds.Tables["Assignments"].AcceptChanges();

            var query = (
                   from assign in ds.Tables["Assignments"].AsEnumerable()
                   from emp in ds.Tables["Employees"].AsEnumerable()
                   from proj in ds.Tables["Projects"].AsEnumerable()
                   where assign.Field<string>("EmpId") == emp.Field<string>("EmpId")
                   where assign.Field<string>("ProjId") == proj.Field<string>("ProjId")
                   select new
                   {
                       EmpId = emp.Field<string>("EmpId"),
                       EmpName = emp.Field<string>("EmpName"),
                       ProjId = proj.Field<string>("ProjId"),
                       ProjName = proj.Field<string>("ProjName"),
                       Eval = assign.Field<Nullable<int>>("Eval")
                   });
            DataTable result = new DataTable();
            result.Columns.Add("EmpId");
            result.Columns.Add("EmpName");
            result.Columns.Add("ProjId");
            result.Columns.Add("ProjName");
            result.Columns.Add("Eval");
            foreach (var x in query)
            {
                object[] allFields = { x.EmpId, x.EmpName, x.ProjId, x.ProjName, x.Eval };
                result.Rows.Add(allFields);
            }
            displayAssign = result;
            return displayAssign;
        }

        internal static int InsertData(string[] a)
        {
            var test = (
                   from assign in ds.Tables["Assignments"].AsEnumerable()
                   where assign.Field<string>("EmpId") == a[0]
                   where assign.Field<string>("ProjId") == a[1]
                   select assign);
            if (test.Count() > 0)
            {
                EmpProj3.Form1.DALMessage("This assignment already exists");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Assignments"].NewRow();
                line.SetField("EmpId", a[0]);
                line.SetField("ProjId", a[1]);
                ds.Tables["Assignments"].Rows.Add(line);

                adapter.Update(ds.Tables["Assignments"]);

                if (displayAssign != null)
                {
                    var query = (
                           from emp in ds.Tables["Employees"].AsEnumerable()
                           from proj in ds.Tables["Projects"].AsEnumerable()
                           where emp.Field<string>("EmpId") == a[0]
                           where proj.Field<string>("ProjId") == a[1]
                           select new
                           {
                               EmpId = emp.Field<string>("EmpId"),
                               EmpName = emp.Field<string>("EmpName"),
                               ProjId = proj.Field<string>("ProjId"),
                               ProjName = proj.Field<string>("ProjName"),
                               Eval =line.Field<Nullable<int>>("Eval")  
                           });
                    // Note that Eval =line.Field<Nullable<int>>("Eval") will 
                    // always place null in Eval. It is not needed. 
                    // It is enough to ommit Eval in the select and  
                    // ommit r.Eval in displayAssign.Rows.Add(...)
                    var r = query.Single();
                    displayAssign.Rows.Add(new object[] { r.EmpId, r.EmpName, r.ProjId, r.ProjName, r.Eval });
                }
                return 0;
            }
            catch (Exception)
            {
                EmpProj3.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
        }

        internal static int UpdateData(string[] a)
        {
            return 0;  //not used
        }

        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                var lines = ds.Tables["Assignments"].AsEnumerable()
                                .Where(s =>
                                   lId.Any(x => (x[0] == s.Field<string>("EmpId") && x[1] == s.Field<string>("ProjId"))));

                foreach (var line in lines)
                {
                    line.Delete();
                }

                adapter.Update(ds.Tables["Assignments"]);

                if (displayAssign != null)
                {
                    foreach (var p in lId)
                    {
                        var r = displayAssign.AsEnumerable()
                                .Where(s => (s.Field<string>("EmpId") == p[0] && s.Field<string>("ProjId") == p[1]))
                                .Single();
                        displayAssign.Rows.Remove(r);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                EmpProj3.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }

        internal static int UpdateEval(string[] a, Nullable<int> eval)
        {
            try
            {
                var line = ds.Tables["Assignments"].AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("EmpId") == a[0] && s.Field<string>("ProjId") == a[1]))
                                    .Single();

                line.SetField("Eval", eval);

                adapter.Update(ds.Tables["Assignments"]);

                if (displayAssign != null)
                {
                    var r = displayAssign.AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("EmpId") == a[0] && s.Field<string>("ProjId") == a[1]))
                                    .Single();
                    r.SetField("Eval", eval);
                }
                return 0;  
            }
            catch (Exception)
            {
                EmpProj3.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }
    }
}

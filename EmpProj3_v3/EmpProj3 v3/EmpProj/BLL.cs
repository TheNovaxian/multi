using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Business
{
    class Employees
    {
        internal static int UpdateEmployees()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Employees"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            // dt typically will be null or have one line
            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidEmpId(r.Field<string>("EmpId"))))
                   //(!IsValidEmpId(dt.Rows[0].Field<string>("EmpId")))
                {
                    EmpProj3.Form1.BLLMessage("Invalid Id for Employees");
                    // for DataRowState.Added it would be enough one of the three solution blow: 
                    //
                    //ds.Tables["Employees"].RejectChanges();
                    // or
                    //ds.Tables["Employees"].Rows.Find(dt.Rows[0].Field<string>("EmpId")).Delete();
                    //ds.Tables["Employees"].AcceptChanges();
                    // or
                    //ds.Tables["Employees"].Rows.Remove(ds.Tables["Employees"].Rows.Find(dt.Rows[0].Field<string>("EmpId")));
                    //
                    // But DataRowState.Modified, the row may have a daughter row to which the new key (EmpId) is already propagated.
                    // So we can not RejectChanges() at datatable level. So we must use RejectChanges() at dataset level:
                    ds.RejectChanges();
                    return -1;
                }                
                else if (dt.Select("SALARY < 15000").Length > 0)
                {
                    EmpProj3.Form1.BLLMessage("Employee Insertion/Update rejected: Salary less than 15000");
                    ds.Tables["Employees"].RejectChanges();
                    // or
                    //ds.Tables["Employees"].Rows.Find(dt.Rows[0].Field<string>("EmpId")).Delete();
                    //ds.Tables["Employees"].AcceptChanges();
                    // or
                    //ds.Tables["Employees"].Rows.Remove(ds.Tables["Employees"].Rows.Find(dt.Rows[0].Field<string>("EmpId"))); 
                    return -1;
                }
                else
                {
                    return Data.Employees.UpdateEmployees();
                }
            }    
            else
            {
                return Data.Employees.UpdateEmployees();
            }
        }

        private static bool IsValidEmpId(string empId)
        {
            bool r = true;
            if (empId.Length != 5) { r = false; }
            else if (empId[0] != 'E') { r = false; }
            else
            {
                for (int i = 1; i < empId.Length; i++)
                {
                    r = r && Char.IsDigit(empId[i]);
                }
            }
            return r;
        }
    }

    class Projects
    {
        internal static int UpdateProjects()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Projects"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            // dt typically will be null or have one line
            if (dt != null)
            {
                if (dt.AsEnumerable().Any(r => !IsValidProjId(r.Field<string>("ProjId"))))
                   //(!IsValidProjId(dt.Rows[0].Field<string>("ProjId")))
                {
                    EmpProj3.Form1.BLLMessage("Invalid Id for Projects");
                    // for DataRowState.Added it would be enough one of the three solution blow: 
                    //
                    //ds.Tables["Projects"].RejectChanges();
                    // or
                    //ds.Tables["Projects"].Rows.Find(dt.Rows[0].Field<string>("ProjId")).Delete();
                    //ds.Tables["Projects"].AcceptChanges();
                    // or
                    //ds.Tables["Projects"].Rows.Remove(ds.Tables["Projects"].Rows.Find(dt.Rows[0].Field<string>("ProjId")));
                    //
                    // But DataRowState.Modified, the row may have a daughter row to which the new key (ProjId) is already propagated.
                    // So we can not RejectChanges() at datatable level. So we must use RejectChanges() at dataset level:
                    ds.RejectChanges();
                    return -1;
                }

                else if (dt.Select("Duration < 3").Length > 0)
                {
                    EmpProj3.Form1.BLLMessage("Project Insertion/Update rejected: Duration less than 3");
                    ds.Tables["Projects"].RejectChanges();
                    // or
                    //ds.Tables["Projects"].Rows.Find(dt.Rows[0].Field<string>("ProjId")).Delete();
                    //ds.Tables["Projects"].AcceptChanges();
                    // or
                    //ds.Tables["Projects"].Rows.Remove(ds.Tables["Projects"].Rows.Find(dt.Rows[0].Field<string>("ProjId"))); 
                    return -1;
                }
                else
                {
                    return Data.Projects.UpdateProjects();
                }
            }
            else
            {
                return Data.Projects.UpdateProjects();
            }
        }

        private static bool IsValidProjId(string projId)
        {
            bool r = true;
            if (projId.Length != 4) { r = false; }
            else if (projId[0] != 'P') { r = false; }
            else
            {
                for (int i = 1; i < projId.Length; i++)
                {
                    r = r && Char.IsDigit(projId[i]);
                }
            }
            return r;
        }
    }

    class Assignments
    {
        internal static int UpdateEvaluation(string[] a, string ev)
        {
            Nullable<int> eval;
            int temp;

            if (ev == "")
            {
                eval = null;                
            }           
            else if (int.TryParse(ev, out temp) && (0<=temp && temp <= 100))
            {
                eval = temp;               
            }
            else
            {
                EmpProj3.Form1.BLLMessage(
                          "Evaluation must be an integer between 0 and 100"
                          );
                return -1;
            }
            
            return Data.Assignments.UpdateEval(a,eval);                 
        }
    }
}

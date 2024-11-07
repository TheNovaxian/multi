using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmpProj3
{
    public partial class Form1 : Form
    {
        internal enum Grids
        {
            Emp,
            Proj,
            Assign
        }

        internal static Form1 current;

        private Grids grid;

        private bool OKToChange = true;

        public Form1()
        {
            current = this;
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2();
            Form2.current.Visible = false;

            Text = "Employees & Projects";
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                grid = Grids.Emp;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource1.DataSource = Data.Employees.GetEmployees();
                bindingSource1.Sort = "EmpId";
                dataGridView1.DataSource = bindingSource1;

                dataGridView1.Columns["EmpName"].HeaderText = "Employee Name";
                dataGridView1.Columns["EmpId"].DisplayIndex = 0;
                dataGridView1.Columns["EmpName"].DisplayIndex = 1;
                dataGridView1.Columns["Salary"].DisplayIndex = 2;
            }           
        }

        private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                grid = Grids.Proj;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource2.DataSource = Data.Projects.GetProjects();
                bindingSource2.Sort = "ProjId";
                dataGridView1.DataSource = bindingSource2;

                dataGridView1.Columns["ProjName"].HeaderText = "Project Name";
                dataGridView1.Columns["ProjId"].DisplayIndex = 0;
                dataGridView1.Columns["ProjName"].DisplayIndex = 1;
                dataGridView1.Columns["Duration"].DisplayIndex = 2;
            }            
        }

        private void assignmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange && (grid != Grids.Assign))
            {
                grid = Grids.Assign;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource3.DataSource = Data.Assignments.GetDisplayAssignments();
                bindingSource3.Sort = "EmpId, ProjId";    // using bindingSource to sort by two columns
                dataGridView1.DataSource = bindingSource3;
                //dataGridView1.Sort(dataGridView1.Columns["EmpId"], ListSortDirection.Ascending);

                dataGridView1.Columns["EmpName"].HeaderText = "Employee Name";
                dataGridView1.Columns["ProjName"].HeaderText = "Project Name";
                dataGridView1.Columns["Eval"].HeaderText = "Evaluation";
                dataGridView1.Columns["EmpId"].DisplayIndex = 0;
                dataGridView1.Columns["EmpName"].DisplayIndex = 1;
                dataGridView1.Columns["ProjId"].DisplayIndex = 2;
                dataGridView1.Columns["ProjName"].DisplayIndex = 3;
                dataGridView1.Columns["Eval"].DisplayIndex = 4;
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            if (Business.Employees.UpdateEmployees() == -1)
            {
                // It should not be necessary, but sometimes 
                // the bindingSource does not reduce the size of 
                // the datagrid and keeps giving DataError for not finding 
                // enough rows in the data table
                // to prevent this situation, we use the four lines below
                //bindingSource1.Sort = "";
                //bindingSource1.DataSource = null;
                //bindingSource1.DataSource = Data.Employees.GetEmployees();
                //bindingSource1.Sort = "EmpId";
                bindingSource1.ResetBindings(false);
            }
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e) 
        {
            if (Business.Projects.UpdateProjects() == -1)
            {
                // It should not be necessary, but sometimes 
                // the bindingSource does not reduce the size of 
                // the datagrid and keeps giving DataError for not finding 
                // enough rows in the data table
                // to prevent this situation, we use the four lines below
                //bindingSource2.Sort = "";
                //bindingSource2.DataSource = null;
                //bindingSource2.DataSource = Data.Projects.GetProjects();
                //bindingSource2.Sort = "ProjId";
                bindingSource2.ResetBindings(false);
            }
        }

        private void menuStrip1_Click(object sender, EventArgs e)
        {
            // =========================================================================
            // If the insertion / update is ended by just changing to another table 
            // (clicking on the menu strip) without clicking on datagrid, we need
            // this event to ensure the database is updated. 
            // This applies tables that have insertions / updates directly from the 
            // datagridview: Employees and Projects.
            // =========================================================================  
            OKToChange = true;
            Validate();
            if (grid==Grids.Emp)
            {
                //// It forces any current edition in DataGridView to be transmitted to the Datatable.
                //dataGridView1.DataSource = null;
                //// Ensure Employees keeps showing, if bindingSource1 is initialized.
                //dataGridView1.DataSource = bindingSource1;
                if (Business.Employees.UpdateEmployees() == -1)
                {                    
                    OKToChange = false;
                }
            }
            else if (grid == Grids.Proj)
            {
                //// It forces any current edition in DataGridView to be transmitted to the Datatable.
                //dataGridView1.DataSource = null;
                //// Ensure Projects keeps showing, if bindingSource2 is initialized.
                //dataGridView1.DataSource = bindingSource2;
                if (Business.Projects.UpdateProjects() == -1)
                {
                    OKToChange = false;
                }               
            }
        }

        internal static void BLLMessage(string s)
        {
            MessageBox.Show("Business Layer: " + s);
        }

        internal static void DALMessage(string s)
        {
            MessageBox.Show("Data Layer: " + s);
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.INSERT,null);            
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {                
                if (""+c[0].Cells["Eval"].Value == "")
                {
                    Form2.current.Start(Form2.Modes.UPDATE, c);
                }
                else
                {
                    MessageBox.Show("To update this line, Eval value must be removed first.");
                }                             
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion");
            }
            else // (c.Count > 1)
            {
                List<string[]> lId = new List<string[]>();
                for (int i = 0; i < c.Count; i++)
                {       
                    lId.Add(new string[] { "" + c[i].Cells["EmpId"].Value,
                                           "" + c[i].Cells["ProjId"].Value });
                }
                Data.Assignments.DeleteData(lId);
            }
        }

        private void evaluationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for evaluation update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form2.current.Start(Form2.Modes.EVALUATION, c);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update");
            e.Cancel = false;  // Ensure automatic undoing of the error
            OKToChange = false;
        } 
    }
}

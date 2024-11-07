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
    public partial class Form2 : Form
    {
        internal enum Modes
        {
            INSERT,
            UPDATE,
            EVALUATION
        }

        internal static Form2 current;

        private Modes mode = Modes.INSERT;

        private string[] assignInitial;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;

            comboBox1.DisplayMember = "EmpId";
            comboBox1.ValueMember = "EmpId";
            comboBox1.DataSource = Data.Employees.GetEmployees();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

            comboBox2.DisplayMember = "ProjId";
            comboBox2.ValueMember = "ProjId";
            comboBox2.DataSource = Data.Projects.GetProjects();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.Enabled = false;

            if (((mode == Modes.UPDATE) || (mode == Modes.EVALUATION)) && (c!=null))
            {
                comboBox1.SelectedValue = c[0].Cells["EmpId"].Value;
                comboBox2.SelectedValue = c[0].Cells["ProjId"].Value;
                textBox3.Text = ""+c[0].Cells["Eval"].Value;
                assignInitial = new string[] { (string)c[0].Cells["EmpId"].Value, (string)c[0].Cells["ProjId"].Value };
            }
            if (mode == Modes.UPDATE)
            {
                textBox3.Enabled = false;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
            if (mode == Modes.EVALUATION)
            {
                textBox3.Enabled = true;
                textBox3.ReadOnly = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }

            ShowDialog();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in Data.Employees.GetEmployees().AsEnumerable()
                        where r.Field<string>("EmpId") == (string)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("EmpName") };
                textBox1.Text = a.Single().Name;                
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var a = from r in Data.Projects.GetProjects().AsEnumerable()
                        where r.Field<string>("ProjId") == (string)comboBox2.SelectedValue
                        select new { Name = r.Field<string>("ProjName") };
                textBox2.Text = a.Single().Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.INSERT)
            {
                r= Data.Assignments.InsertData(new string[] {(string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
            }
            if (mode == Modes.UPDATE)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(assignInitial);

                r = Data.Assignments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
               
                if (r == 0)
                {
                    r = Data.Assignments.DeleteData(lId);
                }               
            }
            if (mode == Modes.EVALUATION)
            {
                r = Business.Assignments.UpdateEvaluation(assignInitial,textBox3.Text);
            }

            if (r == 0) { Close(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

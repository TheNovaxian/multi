using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultitierProj
{
    public partial class Form2 : Form
    {

        SqlDataAdapter adapter;
        DataSet ds;

        internal enum Modes
        {
            ADD,
            MODIFY
        }

        internal static Form2 current;

        private Modes mode = Modes.ADD;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }


        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;

            if (mode == Modes.ADD)
            {    
               
                textBox1.ReadOnly = false;
                textBox1.Text = "";
                textBox2.ReadOnly = false;
                textBox2.Text = "";


                comboBox1.Enabled = true; // Make sure the ComboBox is enabled
                comboBox1.SelectedIndex = -1; // Clear selection (if needed)

                comboBox2.Enabled = true; // Make sure the ComboBox is enabled
                comboBox2.SelectedIndex = -1;
            }

            if(mode == Modes.MODIFY)
            {
                textBox1.ReadOnly = true;
                textBox1.Text = "" + c[0].Cells["StName"].Value;
                textBox2.ReadOnly = false;
                textBox2.Text = "" + c[0].Cells["CName"].Value;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder
            {
                DataSource = "(local)",
                InitialCatalog = "College1en",
                UserID = "sa",
                Password = "sysadm"
            };

            string query = @"
        SELECT 
            E.StId, 
            S.StName, 
            E.CId, 
            C.CName 
        FROM 
            Enrollments E
        JOIN 
            Students S ON E.StId = S.StId
        JOIN 
            Courses C ON E.CId = C.CId";

            adapter = new SqlDataAdapter(query, cs.ConnectionString);
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            ds = new DataSet();
            adapter.Fill(ds, "Enrollments");

            // Populate comboBox1 for Students
            comboBox1.DisplayMember = "StId"; // Display the student name
            comboBox1.ValueMember = "StId"; // Value is the student ID
            comboBox1.DataSource = ds.Tables["Enrollments"].DefaultView.ToTable(true, "StId", "StName"); // Distinct students
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // Populate comboBox2 for Courses
            comboBox2.DisplayMember = "CId"; // Display the course name
            comboBox2.ValueMember = "CId"; // Value is the course ID
            comboBox2.DataSource = ds.Tables["Enrollments"].DefaultView.ToTable(true, "CId", "CName"); // Distinct courses
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var selectedStudentId = comboBox1.SelectedValue.ToString();
                var studentRow = ds.Tables["Enrollments"].AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("StId") == selectedStudentId);

                if (studentRow != null)
                {
                    textBox1.Text = studentRow.Field<string>("StName"); // Display student name in textBox1
                }
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var selectedCourseId = comboBox2.SelectedValue.ToString();
                var courseRow = ds.Tables["Enrollments"].AsEnumerable()
                    .FirstOrDefault(s => s.Field<string>("CId") == selectedCourseId);

                if (courseRow != null)
                {
                    textBox2.Text = courseRow.Field<string>("CName"); // Display course name in textBox2
                }
            }

        }
    }
}

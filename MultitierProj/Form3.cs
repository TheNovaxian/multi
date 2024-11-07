using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Data;
using College1en;

namespace MultitierProj
{
    public partial class Form3 : Form
    {
        internal enum Modes
        {
            FINAL
        }

        internal static Form3 current;

        private Modes mode = Modes.FINAL;

        private string[] assignInitial;

        public Form3()
        {
            current = this;
            InitializeComponent();
        }

        private static SqlDataAdapter InitializeDataAdapter()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY StId, CId",
                Connect.ConnectionString
                );

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            //builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }
        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = false;

            if (mode == Modes.FINAL)
            {
                // Assuming 'c' contains a single selected row for a student or enrollment
                if (c.Count > 0)
                {
                    var selectedRow = c[0];
                    string studentId = selectedRow.Cells["StId"].Value.ToString(); // Example: Assume StId is the student ID field in the DataGridView

                    // Fetch the data from the database
                    LoadEnrollmentData(studentId);
                }
            }

            ShowDialog();
        }

        private void LoadEnrollmentData(string studentId)
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConnectionString))
            {
                // SQL query with JOIN to fetch data from Enrollments, Students, and Courses tables
                string query = @"
            SELECT e.StId, s.StName, e.CId, c.Cname, e.FinalGrade
            FROM Enrollments e
            INNER JOIN Students s ON e.StId = s.StId
            INNER JOIN Courses c ON e.CId = c.CId
            WHERE e.StId = @StId";

                SqlDataAdapter r = new SqlDataAdapter(query, connection);
                r.SelectCommand.Parameters.AddWithValue("@StId", studentId);

                DataTable enrollmentData = new DataTable();
                r.Fill(enrollmentData);

                if (enrollmentData.Rows.Count > 0)
                {
                    DataRow row = enrollmentData.Rows[0];
                    textBox1.Text = row["StId"].ToString();  // Student ID
                    textBox4.Text = row["StName"].ToString();  // Student Name from the Students table
                    textBox3.Text = row["CId"].ToString();  // Course ID
                    textBox2.Text = row["Cname"].ToString();  // Course Name from the Courses table
                    textBox5.Text = row["FinalGrade"].ToString();  // Final Grade
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (mode == Modes.FINAL)
            {
                int finalGrade = int.Parse(textBox5.Text);
                Data.Enrollments.UpdateFinal(assignInitial, finalGrade);
            }
        }



    }
}

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

            var selectedRow = c[0];
            textBox1.Text = selectedRow.Cells["StId"].Value.ToString();
            textBox4.Text = selectedRow.Cells["StName"].Value.ToString();
            textBox3.Text = selectedRow.Cells["CId"].Value.ToString();
            textBox2.Text = selectedRow.Cells["CName"].Value.ToString();
            textBox5.Text = selectedRow.Cells["FinalGrade"].Value.ToString();


            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = false;

            //if (mode == Modes.FINAL)
            //{
               
            //    if (c.Count > 0)
            //    {
            //        //var selectedRow = c[0];
            //        string studentId = selectedRow.Cells["StId"].Value.ToString(); 
                    
            //        //LoadEnrollmentData(studentId);
            //    }
            //}

            ShowDialog();
        }

      

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (mode == Modes.FINAL)
            {
                int? finalGrade = null;



                if (textBox5.Text != "")
                {
                    if(int.TryParse(textBox5.Text, out int temp)){
                        finalGrade = temp;
                    }
                    else
                    {
                        College1en.Form1.BLLMessage("Final Grade must be an integer between 0 and 100 or null.");
                        return;
                    }
                    
                }
                assignInitial = new string[] { textBox1.Text, textBox3.Text };
                Data.Enrollments.UpdateFinal(assignInitial, finalGrade);
            }
            Close();
        }



    }
}

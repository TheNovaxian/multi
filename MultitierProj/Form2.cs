﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MultitierProj
{
    public partial class Form2 : Form
    {

        internal enum Modes
        {
            ADD,
            MODIFY,
            FINAL
        }

        internal static Form2 current;

        private Modes mode = Modes.ADD;

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

            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = Data.Students.GetStudents();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = Data.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            comboBox1.Enabled = true;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;


            if (((mode == Modes.MODIFY) || (mode == Modes.FINAL)) && (c != null))
            {
                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;
                textBox3.Text = "" + c[0].Cells["FinalGrade"].Value;
                assignInitial = new string[] { "" + c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
                //var selectedRow = c[0];
                //comboBox1.DisplayMember = "StId";
                //comboBox1.ValueMember = "StId";
                //comboBox1.DataSource = Data.Students.GetStudents();
                //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                //comboBox1.SelectedValue = selectedRow.Cells["StId"].Value.ToString(); ;

                //comboBox2.DisplayMember = "CId";
                //comboBox2.ValueMember = "CId";
                //comboBox2.DataSource = Data.Courses.GetCourses();
                //comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                //comboBox2.SelectedValue = selectedRow.Cells["CId"].Value.ToString(); ;

                //comboBox1.Enabled = false;
                //textBox1.ReadOnly = true;
                //textBox2.ReadOnly = false;
                //textBox3.ReadOnly = true;
            }
            if (mode == Modes.MODIFY)
            {
                //textBox3.Enabled = false;
                textBox3.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox1.ReadOnly = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = true;
            }
            if (mode == Modes.FINAL)
            {
                textBox3.Enabled = true;
                textBox3.ReadOnly = false;
                textBox2.ReadOnly = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }

            ShowDialog();
        }

      

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in Data.Students.GetStudents().AsEnumerable()
                        where r.Field<string>("StId") == (string)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("StName") };
                textBox1.Text = a.Single().Name;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var a = from r in Data.Courses.GetCourses().AsEnumerable()
                        where r.Field<string>("CId") == (string)comboBox2.SelectedValue
                        select new { Name = r.Field<string>("CName") };
                textBox2.Text = a.Single().Name;
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.ADD)
            {
                r = Data.Enrollments.AddData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
            }
            if (mode == Modes.MODIFY)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(assignInitial);

                r = Data.Enrollments.AddData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });

                if (r == 0)
                {
                    r = Data.Enrollments.DeleteData(lId);
                }
            }
            if (mode == Modes.FINAL)
            {
                r = BusinessLayer.Enrollments.UpdateFinalGrade(assignInitial, textBox3.Text);
            }

            if (r == 0) { Close(); }
        }
    }
}

﻿using MultitierProj;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace College1en
{
    public partial class Form1 : Form
    {
        internal static Form1 current;

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
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource1.DataSource = Data.Programs.GetPrograms();
                bindingSource1.Sort = "ProgId";
                dataGridView1.DataSource = bindingSource1;
            }
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource2.DataSource = Data.Courses.GetCourses();
                bindingSource2.Sort = "CId";
                dataGridView1.DataSource = bindingSource2;

                dataGridView1.Columns["CId"].HeaderText = "Course ID";
                dataGridView1.Columns["CId"].DisplayIndex = 0;
                dataGridView1.Columns["CName"].DisplayIndex = 1;
                dataGridView1.Columns["ProgId"].DisplayIndex = 2;
             
            }
        }

        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource3.DataSource = Data.Enrollments.GetEnrollments();
                bindingSource3.Sort = "CId";
                dataGridView1.DataSource = bindingSource3;

                dataGridView1.Columns["StId"].HeaderText = "Students ID";
                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["CId"].DisplayIndex = 1;
                dataGridView1.Columns["FinalGrade"].DisplayIndex = 2;

            }
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OKToChange)
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource4.DataSource = Data.Students.GetStudents();
                bindingSource4.Sort = "StId";
                dataGridView1.DataSource = bindingSource4;

                dataGridView1.Columns["StId"].HeaderText = "Students ID";
                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["ProgId"].DisplayIndex = 2;

            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            OKToChange = true;

            BusinessLayer.Programs.UpdatePrograms();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            if (BusinessLayer.Courses.UpdateCourses() == -1) {
                Validate();
            }
        }

        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            if(BusinessLayer.Enrollments.UpdateEnrollments() == -1)
            {
                Validate();
            }
        }

        private void bindingSource4_CurrentChanged(object sender, EventArgs e)
        {
            if(BusinessLayer.Students.UpdateStudents() == -1) { Validate(); }
        }

        private void menuStrip1_Click(object sender, EventArgs e)
        {
            OKToChange = true;

            BindingSource temp = (BindingSource)dataGridView1.DataSource;
            Validate();

            if (temp == bindingSource1)
            {
                if (BusinessLayer.Programs.UpdatePrograms() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource2)
            {
                if (BusinessLayer.Courses.UpdateCourses() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource3)
            {
                if (BusinessLayer.Enrollments.UpdateEnrollments() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource4)
            {
                if (BusinessLayer.Students.UpdateStudents() == -1)
                {
                    OKToChange = false;
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
            e.Cancel = false; 
                               
            OKToChange = false;
        }

        internal static void msgCommandTooLow()
        {
            MessageBox.Show("Commande rejetée: inférieur à 10.00 Can$");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BindingSource temp = (BindingSource)dataGridView1.DataSource;

            if (temp == bindingSource1)
            {
                if (BusinessLayer.Programs.UpdatePrograms() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource2)
            {
                if (BusinessLayer.Courses.UpdateCourses() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource3)
            {
                if (BusinessLayer.Enrollments.UpdateEnrollments() == -1)
                {
                    OKToChange = false;
                }
            }
            else if (temp == bindingSource4)
            {
                if (BusinessLayer.Students.UpdateStudents() == -1)
                {
                    OKToChange = false;
                }
            }

            if (!OKToChange)
            {
                e.Cancel = true;
                OKToChange = true;
            }

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.ADD, null);
            Form2.current.Show();
        }
    }
}
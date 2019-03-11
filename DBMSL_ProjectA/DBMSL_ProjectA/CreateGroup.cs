﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace DBMSL_ProjectA
{
    public partial class CreateGroup : Form
    {
        public CreateGroup()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            string RegNo = txtRegistrationNo.Text;
            bool IsConnnected = DatabaseConnection.start();
            DatabaseConnection.createStatement("Select Id from Student where RegistrationNo = '" + RegNo + "'");
            SqlDataReader reader = DatabaseConnection.getData();
            string StudentId = "";
            if (reader.Read())
            {
                StudentId = reader.GetValue(reader.GetOrdinal("Id")).ToString();
                MessageBox.Show(StudentId);
            }
            DatabaseConnection.createStatement("Select * from Person where Id = " + StudentId );
            reader = DatabaseConnection.getData();
            Student student = new Student();
            student.RegistrationNo = RegNo;
            student.StudentId = int.Parse(StudentId);
            if (reader.Read())
            {
                student.FirstName =  reader.GetString(1).ToString();
                student.LastName =  reader.GetString(2).ToString();
                student.Contact = reader.GetString(3).ToString();
                student.Email = reader.GetString(4).ToString();
                student.DateOfBirth = Convert.ToDateTime(reader.GetDateTime(5));
            }
            TempData.add_GroupStudent(student);
            gvStudents.Rows.Clear();
            gvStudents.Refresh();
            foreach(Student s in TempData.GetGroupStudents())
            {
                gvStudents.Rows.Add(s.RegistrationNo, s.FirstName);
            }
            

        }

        private void txtCreateGroup_Click(object sender, EventArgs e)
        {
            bool IsConnnected = DatabaseConnection.start();
            foreach (Student s in TempData.GetGroupStudents())
            {
                MessageBox.Show(s.StudentId.ToString());
                DatabaseConnection.createStatement("Insert into GroupStudent (GroupId, StudentId ,Status , AssignmentDate) " +
                    "Values (7," + s.StudentId.ToString() + ", 3 ,'" + DateTime.Now.ToString("yyyy-MM-dd") + "')");
                DatabaseConnection.performAction();
            }
            TempData.Clear_GroupStudents();
        }
    }
}

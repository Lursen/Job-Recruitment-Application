using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
    public partial class EmployeeForm : Form
    {
        bool changeCount = false;
        public EmployeeForm()
        {
            InitializeComponent();

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            label1.Text = "Пользователь:";
            textBox1.Text = UserManipulation.CurrentUser;

            label2.Text = "ФИО:";
            textBox2.Text = UserManipulation.GetUsersData("Name");

            label3.Text = "Портфолио:";
            textBox3.Text = UserManipulation.GetUsersData("Portfolio");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (changeCount == false)
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;

                changeCount = true;
            }
            else
            {
                UserManipulation.ChangeUsersData(textBox1.Text, "Email");
                UserManipulation.ChangeUsersData(textBox2.Text, "Name");
                UserManipulation.ChangeUsersData(textBox3.Text, "Portfolio");

                UserManipulation.CurrentUser = textBox1.Text;

                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;

                changeCount = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FindVacancy findForm = new FindVacancy();
            findForm.ShowDialog();
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {

        }
    }
}

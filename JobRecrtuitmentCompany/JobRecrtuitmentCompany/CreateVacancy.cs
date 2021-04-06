﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
    public partial class CreateVacancy : Form
    {
        public CreateVacancy()
        {
            InitializeComponent();

            label1.Text = "Название вакансии:";
            label2.Text = "Заработная плата:";
            label3.Text = "Тип работ:";
            label4.Text = "Требования:";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SampleDbContext context = new SampleDbContext())
            {
                //var employerId = context.Users.Where(u => u.Email == UserManipulation.CurrentUser).Select(u => u.UserId).SingleOrDefault();
                Employer employer = (Employer)context.Users.Where(u => u.Email == UserManipulation.CurrentUser).SingleOrDefault();
                if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    Vacancy vacancy = new Vacancy { Employer = employer, Name = textBox1.Text, Salary = int.Parse(textBox2.Text), Type = textBox3.Text, Requirements = textBox4.Text };
                    context.Vacancies.Add(vacancy);
                    context.SaveChanges();
                    Close();
                }
            }

        }
    }
}

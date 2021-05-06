using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Entity;

namespace JobRecrtuitmentCompany
{
    public partial class WatchVacancyForm : Form
    {
        public WatchVacancyForm()
        {
            InitializeComponent();

            label1.Text = "Название вакансии:";
            label2.Text = "Заработная плата:";
            label3.Text = "Тип работ:";
            label4.Text = "Требования:";
            
            var vacancy = VacancyFinder.GetVacancy(VacancyFinder.currentVacancy);

            textBox1.Text = vacancy.Name;
            textBox2.Text = vacancy.Salary.ToString();
            textBox3.Text = vacancy.Type;
            textBox4.Text = vacancy.Requirements;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var currentVacancy = VacancyFinder.GetVacancy(VacancyFinder.currentVacancy);
            var responses = currentVacancy.EmployeesResponses.ToList();
            Employee curUser = (Employee)UserManipulation.GetUser(UserManipulation.CurrentUser);

            bool responseExists = responses.Contains(curUser);

            if (responseExists)
            {
                MessageBox.Show("Отклик уже оставлен!");
            }
            else
            {
                VacancyFinder.AddResponse(currentVacancy,curUser);
            }
        }
    }
}

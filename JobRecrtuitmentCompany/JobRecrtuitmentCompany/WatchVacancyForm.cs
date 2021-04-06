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

            using (SampleDbContext context = new SampleDbContext())
            {
                var vacancy = context.Vacancies.Find(VacancyFinder.currentVacancy);

                textBox1.Text = vacancy.Name;
                textBox2.Text = vacancy.Salary.ToString();
                textBox3.Text = vacancy.Type;
                textBox4.Text = vacancy.Requirements;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SampleDbContext context = new SampleDbContext())
            {
                var currentVacancy = context.Vacancies
                  .Include(x => x.EmployeesResponses)
                  .Where(x => x.VacancyId == VacancyFinder.currentVacancy)
                  .FirstOrDefault();

                var responses = currentVacancy.EmployeesResponses.ToList();

                Employee curUser = (Employee)context.Users.Where(x => x.Email == UserManipulation.CurrentUser).FirstOrDefault();
                bool responseExists = responses.Contains(curUser);

                if (responseExists)
                {
                    MessageBox.Show("Отклик уже оставлен!");
                }
                else
                {
                    currentVacancy.EmployeesResponses.Add(curUser);
                    context.SaveChanges();
                }
            }
        }
    }
}

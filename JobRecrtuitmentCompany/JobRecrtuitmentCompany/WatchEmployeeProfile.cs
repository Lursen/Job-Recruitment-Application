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
    public partial class WatchEmployeeProfile : Form
    {
        public WatchEmployeeProfile()
        {
            InitializeComponent();
         
            this.Text = "Информация о соискателе";

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            label1.Text = "Email:";
            label2.Text = "ФИО:";
            label3.Text = "Портфолио:";

            Employee employee = (Employee)UserManipulation.GetUser(VacancyFinder.currentEmployee);

            textBox1.Text = employee.Email;
            textBox2.Text = employee.Name;
            textBox3.Text = employee.Portfolio;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SampleDbContext context = new SampleDbContext())
            {
                var vacancy = context.Vacancies
                               .Include(x => x.EmployeesResponses)
                               .Where(x => x.VacancyId == VacancyFinder.currentVacancy)
                               .FirstOrDefault();

                var tagToRemove = vacancy.EmployeesResponses
                    .Single(x => x.Email == VacancyFinder.currentEmployee);

                vacancy.EmployeesResponses.Remove(tagToRemove);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}

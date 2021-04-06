using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
    public partial class ChangeVacancyForm : Form
    {
        public ChangeVacancyForm()
        {
            InitializeComponent();

            label1.Text = "Название вакансии:";
            label2.Text = "Заработная плата:";
            label3.Text = "Тип работ:";
            label4.Text = "Требования:";
            label5.Text = "Отклики:";

            using (SampleDbContext context = new SampleDbContext())
            {
                var vacancyInfo = context.Vacancies
                           .Include(x => x.EmployeesResponses)
                           .Where(x => x.VacancyId == VacancyFinder.currentVacancy)
                           .FirstOrDefault();

                textBox1.Text = vacancyInfo.Name;
                textBox2.Text = vacancyInfo.Salary.ToString();
                textBox3.Text = vacancyInfo.Type;
                textBox4.Text = vacancyInfo.Requirements;
         
                var responses = vacancyInfo.EmployeesResponses.ToList();

                var filteredResponses = from p in responses
                                        select new
                                        {
                                            Email = p.Email,
                                            ФИО = p.Name
                                        };

                dataGridView1.DataSource = filteredResponses.ToList();
            }
    }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SampleDbContext context = new SampleDbContext())
            {
                var vacancy = context.Vacancies.Find(VacancyFinder.currentVacancy);
                vacancy.Name = textBox1.Text;
                vacancy.Salary = int.Parse(textBox2.Text);
                vacancy.Type = textBox3.Text;
                vacancy.Requirements = textBox4.Text;
                context.Entry(vacancy).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SampleDbContext context = new SampleDbContext())
            {
                var vacancy = context.Vacancies.Find(VacancyFinder.currentVacancy);
                context.Vacancies.Remove(vacancy);
                context.SaveChanges();
            }
            Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (!row.IsNewRow)
            {
                VacancyFinder.currentEmployee = (string)row.Cells["Email"].Value;
                WatchEmployeeProfile watchEmployeeProfile = new WatchEmployeeProfile();
                watchEmployeeProfile.ShowDialog();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
    public partial class FindVacancy : Form
    {
        public FindVacancy()
        {
            InitializeComponent();

            label1.Text = "Профессия:";
            label2.Text = "Профессиональная область:";
            label3.Text = "Минимальная заработная плата:";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Job = textBox1.Text;
            string Field = textBox2.Text;
            string Salary = textBox3.Text;

            var filteredVacancies = VacancyFinder.FindVacancy(Job,Field,Salary);

            var vacancies = from p in filteredVacancies
                            select new
                            {
                                Номер_вакансии = p.VacancyId,
                                Название = p.Name,
                                Тип_работ = p.Type,
                                Зарплата = p.Salary
                            };
            dataGridView1.DataSource = vacancies.ToList();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (!row.IsNewRow)
            {
                VacancyFinder.currentVacancy = (int)row.Cells["Номер_вакансии"].Value;
                WatchVacancyForm watchVacancyForm = new WatchVacancyForm();
                watchVacancyForm.ShowDialog();
            }
        }
    }
}

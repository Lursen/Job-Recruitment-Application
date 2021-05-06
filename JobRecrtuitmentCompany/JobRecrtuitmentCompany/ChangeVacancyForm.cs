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


            var vacancyInfo = VacancyFinder.GetVacancy(VacancyFinder.currentVacancy);

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

        private void button1_Click(object sender, EventArgs e)
        {
            VacancyFinder.ChangeVacancy(VacancyFinder.currentVacancy, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VacancyFinder.RemoveVacancy(VacancyFinder.currentVacancy);
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

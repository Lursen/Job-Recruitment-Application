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

    public partial class EmployerForm : Form
    {
        bool changeCount = false;
        public EmployerForm()
        {
            InitializeComponent();
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
                UserManipulation.ChangeUsersData(textBox1.Text,"Email");
                UserManipulation.ChangeUsersData(textBox2.Text, "Company");
                UserManipulation.ChangeUsersData(textBox3.Text, "Requisites");

                UserManipulation.CurrentUser = textBox1.Text;

                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;

                changeCount = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreateVacancy createVacancy = new CreateVacancy();
            createVacancy.ShowDialog();
            UpdatePage();
        }

        private void EmployerForm_Load(object sender, EventArgs e)
        {
            UpdatePage();
        }

        private void UpdatePage()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            label1.Text = "Пользователь:";
            textBox1.Text = UserManipulation.CurrentUser;

            label2.Text = "Компания:";
            textBox2.Text = UserManipulation.GetUsersData("Company");

            label3.Text = "Реквизиты:";
            textBox3.Text = UserManipulation.GetUsersData("Requisites");

            label5.Text = "Вакансии:";

            using (SampleDbContext context = new SampleDbContext())
            {
                var employerId = context.Users.Where(p => p.Email == UserManipulation.CurrentUser).Select(p => p.UserId).FirstOrDefault();

                var vacancies = from p in context.Vacancies
                                where p.UserId == employerId
                                select new
                                {
                                    Номер_вакансии = p.VacancyId,
                                    Название = p.Name,
                                    Тип_работ = p.Type,
                                    Зарплата = p.Salary
                                };
                dataGridView1.DataSource = vacancies.ToList();
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (!row.IsNewRow)
            {
                VacancyFinder.currentVacancy= (int)row.Cells["Номер_вакансии"].Value;
                ChangeVacancyForm changeVacancyForm = new ChangeVacancyForm();
                changeVacancyForm.ShowDialog();
                UpdatePage();
            }
        }
    }
}

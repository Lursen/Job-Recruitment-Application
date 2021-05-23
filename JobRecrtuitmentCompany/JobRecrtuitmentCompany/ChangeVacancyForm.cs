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

            this.Text = "Редактирование вакансии";

            label1.Text = "Название вакансии:";
            label2.Text = "Заработная плата:";
            label3.Text = "Тип работ:";
            label4.Text = "Требования:";
            label5.Text = "Отклики:";

            UpdatePage();

            // Create a Menu Item  
            MenuStrip MainMenu = new MenuStrip();
            MainMenu.Text = "File Menu";
            MainMenuStrip = MainMenu;
            Controls.Add(MainMenu);

            ToolStripMenuItem PersonalCabinet = new ToolStripMenuItem("User");
            PersonalCabinet.Text = "Личный кабинет";

            ToolStripMenuItem Vacancies = new ToolStripMenuItem("Vacancy");
            Vacancies.Text = "Создать вакансию";

            ToolStripMenuItem Logout = new ToolStripMenuItem("Logout");
            Logout.Text = "Выйти";

            MainMenu.Items.Add(PersonalCabinet);
            MainMenu.Items.Add(Vacancies);
            MainMenu.Items.Add(Logout);

            PersonalCabinet.Click += new EventHandler(this.PersonalCabinetItemClick);
            Vacancies.Click += new EventHandler(this.VacanciesItemClick);
            Logout.Click += new EventHandler(this.LogoutItemClick);
        }

        private void UpdatePage()
        {
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


        private void PersonalCabinetItemClick(object sender, EventArgs e)
        {
            this.Hide();
            EmployerForm mainForm = new EmployerForm();
            mainForm.Closed += (s, args) => this.Close();
            mainForm.Show();
        }

        private void VacanciesItemClick(object sender, EventArgs e)
        {
            this.Hide();
            CreateVacancy createVacancy = new CreateVacancy();
            createVacancy.Closed += (s, args) => this.Close();
            createVacancy.Show();
        }

        private void LogoutItemClick(object sender, EventArgs e)
        {
            this.Hide();
            Form1 Form1 = new Form1();
            Form1.Closed += (s, args) => this.Close();
            Form1.Show();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            VacancyFinder.ChangeVacancy(VacancyFinder.currentVacancy, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            MessageBox.Show("Данные сохранены");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VacancyFinder.RemoveVacancy(VacancyFinder.currentVacancy);
            MessageBox.Show("Вакансия удалена");
            this.Hide();
            EmployerForm mainForm = new EmployerForm();
            mainForm.Closed += (s, args) => this.Close();
            mainForm.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row == null) { }
            else
            {
                VacancyFinder.currentEmployee = (string)row.Cells["Email"].Value;
                WatchEmployeeProfile watchEmployeeProfile = new WatchEmployeeProfile();
                watchEmployeeProfile.ShowDialog();
            }
            UpdatePage();
        }
    }
}

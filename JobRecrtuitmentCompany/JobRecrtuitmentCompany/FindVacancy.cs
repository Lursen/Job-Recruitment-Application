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

            this.Text = "Поиск вакансий";

            label1.Text = "Профессия:";
            label2.Text = "Профессиональная область:";
            label3.Text = "Минимальная заработная плата:";
            // Create a Menu Item  
            MenuStrip MainMenu = new MenuStrip();
            MainMenu.Text = "File Menu";
            MainMenuStrip = MainMenu;
            Controls.Add(MainMenu);

            ToolStripMenuItem PersonalCabinet = new ToolStripMenuItem("User");
            PersonalCabinet.Text = "Личный кабинет";

            ToolStripMenuItem Vacancies = new ToolStripMenuItem("Vacancy");
            Vacancies.Text = "Поиск вакансий";

            ToolStripMenuItem Logout = new ToolStripMenuItem("Logout");
            Logout.Text = "Выйти";

            MainMenu.Items.Add(PersonalCabinet);
            MainMenu.Items.Add(Vacancies);
            MainMenu.Items.Add(Logout);

            PersonalCabinet.Click += new EventHandler(this.PersonalCabinetItemClick);
            Vacancies.Click += new EventHandler(this.VacanciesItemClick);
            Logout.Click += new EventHandler(this.LogoutItemClick);
        }

        private void PersonalCabinetItemClick(object sender, EventArgs e)
        {
            this.Hide();
            EmployeeForm mainForm = new EmployeeForm();
            mainForm.Closed += (s, args) => this.Close();
            mainForm.Show();
        }
        private void VacanciesItemClick(object sender, EventArgs e)
        {

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

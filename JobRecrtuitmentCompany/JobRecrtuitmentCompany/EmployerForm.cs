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

            this.Text = "Личный кабинет";

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

        private void PersonalCabinetItemClick(object sender, EventArgs e)
        {

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
            User user = UserManipulation.GetUser(UserManipulation.CurrentUser);
            User ptnUser = UserManipulation.GetUser(textBox1.Text);

            if (changeCount == false)
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;

                changeCount = true;
            }
            else
            {
                if (ptnUser == null || user.Email == ptnUser.Email)
                {
                    UserManipulation.ChangeUsersData(textBox1.Text, "Email");
                    UserManipulation.ChangeUsersData(textBox2.Text, "Company");
                    UserManipulation.ChangeUsersData(textBox3.Text, "Requisites");

                    UserManipulation.CurrentUser = textBox1.Text;

                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox3.ReadOnly = true;

                    changeCount = false;
                }
                else
                {
                    MessageBox.Show("Аккаунт с таким Email уже существует!");

                    textBox1.Text = UserManipulation.CurrentUser;
                }
            }
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

            Employer employer = (Employer)UserManipulation.GetUser(UserManipulation.CurrentUser);

            label1.Text = "Пользователь:";
            textBox1.Text = UserManipulation.CurrentUser;

            label2.Text = "Компания:";
            textBox2.Text = employer.Company;

            label3.Text = "Реквизиты:";
            textBox3.Text = employer.Requisites;

            label5.Text = "Вакансии:";

            var vacancies = VacancyFinder.GetVacancies();

            var filteredVacancies = from p in vacancies
                                    where p.UserId == employer.UserId
                                    select new
                                    {
                                        Номер_вакансии = p.VacancyId,
                                        Название = p.Name,
                                        Тип_работ = p.Type,
                                        Зарплата = p.Salary
                                    };


            dataGridView1.DataSource = filteredVacancies.ToList();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row == null){ }
            else
            {
                this.Hide();
                VacancyFinder.currentVacancy= (int)row.Cells["Номер_вакансии"].Value;
                ChangeVacancyForm changeVacancyForm = new ChangeVacancyForm();
                changeVacancyForm.Closed += (s, args) => this.Close();
                changeVacancyForm.Show();
               // UpdatePage();
            }
        }
    }
}

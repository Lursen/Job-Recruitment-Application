using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
    public partial class EmployeeForm : Form
    {
        bool changeCount = false;
        public EmployeeForm()
        {
            InitializeComponent();

            this.Text = "Личный кабинет";

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            Employee employee = (Employee)UserManipulation.GetUser(UserManipulation.CurrentUser);

            label1.Text = "Пользователь:";
            textBox1.Text = UserManipulation.CurrentUser;

            label2.Text = "ФИО:";
            textBox2.Text = employee.Name;

            label3.Text = "Портфолио:";
            textBox3.Text = employee.Portfolio;

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

        }
        private void VacanciesItemClick(object sender, EventArgs e)
        {
            this.Hide();
            FindVacancy findForm = new FindVacancy();
            findForm.Closed += (s, args) => this.Close();
            findForm.Show();
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
    }
}

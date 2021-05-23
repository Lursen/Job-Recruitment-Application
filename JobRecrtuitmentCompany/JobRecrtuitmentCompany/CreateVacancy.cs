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
    public partial class CreateVacancy : Form
    {
        public CreateVacancy()
        {
            InitializeComponent();
            
            this.Text = "Создание вакансии";

            label1.Text = "Название вакансии:";
            label2.Text = "Заработная плата:";
            label3.Text = "Тип работ:";
            label4.Text = "Требования:";

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
            this.Hide();
            EmployerForm mainForm = new EmployerForm();
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
            //var employerId = context.Users.Where(u => u.Email == UserManipulation.CurrentUser).Select(u => u.UserId).SingleOrDefault();

            if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                Employer employer = (Employer)UserManipulation.GetUser(UserManipulation.CurrentUser);
                Vacancy vacancy = new Vacancy { Employer = employer, Name = textBox1.Text, Salary = int.Parse(textBox2.Text), Type = textBox3.Text, Requirements = textBox4.Text };
                VacancyFinder.AddVacancy(vacancy);

                this.Hide();
                EmployerForm mainForm = new EmployerForm();
                mainForm.Closed += (s, args) => this.Close();
                mainForm.Show();
            }
        }
    }
}

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
    public partial class WatchVacancyForm : Form
    {
        public WatchVacancyForm()
        {
            InitializeComponent();

            this.Text = "Информация о вакансии";

            label1.Text = "Название вакансии:";
            label2.Text = "Заработная плата:";
            label3.Text = "Тип работ:";
            label4.Text = "Требования:";
            
            var vacancy = VacancyFinder.GetVacancy(VacancyFinder.currentVacancy);

            textBox1.Text = vacancy.Name;
            textBox2.Text = vacancy.Salary.ToString();
            textBox3.Text = vacancy.Type;
            textBox4.Text = vacancy.Requirements;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var found = VacancyFinder.AddResponse(VacancyFinder.currentVacancy, UserManipulation.CurrentUser);

            if (found == 0)
            {
                MessageBox.Show("Отклик уже оставлен!");
            }
            else
            {
                MessageBox.Show("Отклик оставлен!");
            }
        }
    }
}

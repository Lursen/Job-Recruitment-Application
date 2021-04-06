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
    public partial class WatchEmployeeProfile : Form
    {
        public WatchEmployeeProfile()
        {
            InitializeComponent();

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            label1.Text = "Email:";
            label2.Text = "ФИО:";
            label3.Text = "Портфолио:";

            using (SampleDbContext context = new SampleDbContext())
            {
                Employee employee = (Employee)context.Users.Where(x => x.Email == VacancyFinder.currentEmployee).FirstOrDefault();

                textBox1.Text = employee.Email;
                textBox2.Text = employee.Name;
                textBox3.Text = employee.Portfolio;
            }
        }
    }
}

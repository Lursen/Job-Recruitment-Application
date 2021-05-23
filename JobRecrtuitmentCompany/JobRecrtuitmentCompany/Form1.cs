using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Авторизация пользователя";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.ShowDialog();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string login = textBox2.Text;
                string password = textBox1.Text;

                var category = UserManipulation.LogIn(login, password);

                if (category == 1)
                {
                    // вход в аккаунт работодателя
                    this.Hide();
                    EmployerForm mainForm = new EmployerForm();
                    mainForm.Closed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else if (category == 2)
                {
                    // вход в аккаунт соискателя
                    this.Hide();
                    EmployeeForm mainForm = new EmployeeForm();
                    mainForm.Closed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден", "Вход в систему");
                }
            }
        }
    }
}

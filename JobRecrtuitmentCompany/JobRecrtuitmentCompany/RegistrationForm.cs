using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobRecrtuitmentCompany
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            this.Text = "Регистрация";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            if (radioButton1.Checked && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password)) //работодатель
            {
                int fnd = UserManipulation.AddNewEmployer(email, password);

                if (fnd == 1)
                {
                    MessageBox.Show("Аккаунт работодателя успешно создан", "Регистрация");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Аккаунт с таким email уже существует", "Регистрация");
                }
            }

            else if (radioButton2.Checked && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password)) //соискатель
            {
                int fnd = UserManipulation.AddNewEmployee(email, password);

                if (fnd == 1)
                {
                    MessageBox.Show("Аккаунт соискателя успешно создан", "Регистрация");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Аккаунт с таким email уже существует", "Регистрация");
                }
            }

            else
            {
                MessageBox.Show("Заполните все поля!", "Регистрация");
            }

        }
    }
}

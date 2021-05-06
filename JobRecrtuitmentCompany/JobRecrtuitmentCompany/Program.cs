using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobRecrtuitmentCompany
{
    public class Vacancy
    {
        public int VacancyId { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Type { get; set; }
        public string Requirements { get; set; }
        public Employer Employer { get; set; }
        public int UserId { get; set; }
        public List<Employee> EmployeesResponses { get; set; } = new List<Employee>();
    }
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class Employer : User
    {
        public string Company { get; set; }
        public string Requisites { get; set; }
        public List<Vacancy> VacanciesResponses { get; set; } = new List<Vacancy>();
    }
    public class Employee : User
    {
        public string Name { get; set; }
        public string Portfolio { get; set; }
        public List<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
    public static class UserManipulation
    {
        public static string CurrentUser;
        public static int LogIn(string email, string password)
        {
            SampleDbContext context = new SampleDbContext();

            var existingEmail = context.Users.Count(a => a.Email == email);
            var existingPassword = context.Users.Where(u => u.Email == email).Select(u => u.Password).SingleOrDefault();
            if (existingEmail != 0)
            {
                if (password == existingPassword)
                {
                    CurrentUser = email;
                    var employerFound = context.Users.OfType<Employer>().Count(a => a.Email == email);

                    if (employerFound != 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }
        public static int AddNewEmployer(string email, string password)
        {
            SampleDbContext context = new SampleDbContext();
       
            var existingEmail = context.Users.Count(a => a.Email == email);
            if (existingEmail == 0)
            {
                Employer employer = new Employer
                {
                    Email = email,
                    Password = password
                };
                context.Users.Add(employer);
                context.SaveChanges();
                return 1;
            }
            return 0;
        }
        public static int AddNewEmployee(string email, string password)
        {
            SampleDbContext context = new SampleDbContext();

            var existingEmail = context.Users.Count(a => a.Email == email);
            if (existingEmail == 0)
            {
                Employee employee = new Employee
                {
                    Email = email,
                    Password = password
                };
                context.Users.Add(employee);
                context.SaveChanges();
                return 1;
            }

            return 0;
        }
        public static User GetUser(string Email)
        {
            SampleDbContext context = new SampleDbContext();
            var user = context.Users.Where(p => p.Email == Email).FirstOrDefault();
            //var employerInfo = context.Database.SqlQuery<string>("SELECT " + data + " FROM dbo.Users WHERE Email = '" + CurrentUser + "'").FirstOrDefault();
            return user;
        }
        public static int ChangeUsersData(string data, string column)
        {
            SampleDbContext context = new SampleDbContext();
            context.Database.ExecuteSqlCommand("UPDATE dbo.Users SET " + column + " = '" + data + "'" + "WHERE Email = '"+ CurrentUser + "'");
            return 1;
        }
    }
    public static class VacancyFinder
    {
        public static int currentVacancy;
        public static string currentEmployee;

        public static int AddResponse(Vacancy vacancy, Employee employee)
        {
            SampleDbContext context = new SampleDbContext();
            vacancy.EmployeesResponses.Add(employee);
            context.SaveChanges();
            return 1;
        }

        public static int AddVacancy(Vacancy vacancy)
        {
            SampleDbContext context = new SampleDbContext();
            context.Vacancies.Add(vacancy);
            context.SaveChanges();
            return 1;
        }

        public static int ChangeVacancy(int vacancyId, string name, string salary, string type, string requirements)
        {
            SampleDbContext context = new SampleDbContext();
            var vacancy = context.Vacancies.Find(vacancyId);
            vacancy.Name = name;
            vacancy.Salary = int.Parse(salary);
            vacancy.Type = type;
            vacancy.Requirements = requirements;
            context.Entry(vacancy).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return 1;
        }
        public static int RemoveVacancy(int vacancyId)
        {
            SampleDbContext context = new SampleDbContext();
            var vacancy = context.Vacancies.Find(vacancyId);
            context.Vacancies.Remove(vacancy);
            context.SaveChanges();
            return 1;
        }
        public static Vacancy GetVacancy(int VacancyId)
        {
            SampleDbContext context = new SampleDbContext();

            var vacancy = context.Vacancies
                           .Include(x => x.EmployeesResponses)
                           .Where(x => x.VacancyId == VacancyId)
                           .FirstOrDefault();

            return vacancy;
        }
        public static IQueryable<Vacancy> GetVacancies()
        {
            SampleDbContext context = new SampleDbContext();
          
            var employerId = context.Users.Where(p => p.Email == UserManipulation.CurrentUser).Select(p => p.UserId).FirstOrDefault();

            var vacancies = context.Vacancies.Where(p => p.UserId == employerId);

            return vacancies; 
        }
        public static IQueryable<Vacancy> FindVacancy(string Job, string Type, string Salary)
        {
            int salary;

            IQueryable<Vacancy> filteredVacancies;
            SampleDbContext context = new SampleDbContext();
            if (Job != "" && Type !="" && Salary != "")
            {
                 salary = int.Parse(Salary);

                filteredVacancies = from vacancy in context.Vacancies
                                        where
                                         ((Job.Contains(vacancy.Name))  ||
                                         (Type.Contains(vacancy.Type))) &&
                                         (vacancy.Salary > salary)
                                     select vacancy;
                return filteredVacancies;
            }
            else if(Type != "" && Salary != "")
            {
                salary = int.Parse(Salary);

                filteredVacancies = from vacancy in context.Vacancies
                                    where
                                         (Type.Contains(vacancy.Type)) &&
                                         (vacancy.Salary > salary)
                                    select vacancy;
                return filteredVacancies;
            }
            else if (Job != "" && Salary != "")
            {
                salary = int.Parse(Salary);

                filteredVacancies = from vacancy in context.Vacancies
                                    where
                                         (Job.Contains(vacancy.Name)) &&
                                         (vacancy.Salary > salary)
                                    select vacancy;
                return filteredVacancies;
            }
            else if (Job != "" && Type != "")
            {
                filteredVacancies = from vacancy in context.Vacancies
                                    where
                                         Job.Contains(vacancy.Name) ||
                                         Type.Contains(vacancy.Type)
                                    select vacancy;
                return filteredVacancies;
            }
            else if (Type != "")
            {
                filteredVacancies = from vacancy in context.Vacancies
                                    where
                                         (Type.Contains(vacancy.Type))
                                    select vacancy;
                return filteredVacancies;
            }
            else if (Job != "")
            {
                filteredVacancies = from vacancy in context.Vacancies
                                    where
                                         (Job.Contains(vacancy.Name))
                                    select vacancy;
                return filteredVacancies;
            }
            else if (Salary != "")
            {
                salary = int.Parse(Salary);

                filteredVacancies = from vacancy in context.Vacancies
                                    where
                                         (vacancy.Salary > salary)
                                    select vacancy;
                return filteredVacancies;
            }
            else
            {
                filteredVacancies = from vacancy in context.Vacancies
                                    select vacancy;
                return filteredVacancies;
            }
        }
    }
    public class SampleDbContext : DbContext
    {
        public SampleDbContext() : base("JRC") { }
        public DbSet<User> Users { get; set; } // Таблица Users
        public DbSet<Vacancy> Vacancies { get; set; } // Таблица Vacancies
    }

    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

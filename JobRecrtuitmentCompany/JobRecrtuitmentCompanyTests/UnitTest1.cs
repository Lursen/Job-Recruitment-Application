using JobRecrtuitmentCompany;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;
using Effort;

namespace JobRecrtuitmentCompanyTests
{
    public class UserService
    {
        private SampleDbContext _context;

        public UserService(SampleDbContext context)
        {
            _context = context;
        }

        public User AddUser(string name, string password)
        {
            var blog = _context.Users.Add(new User { Email = name, Password = password });
            _context.SaveChanges();

            return blog;
        }

        public int LogIn(string email, string password)
        {
            var existingEmail = _context.Users.Count(a => a.Email == email);
            var existingPassword = _context.Users.Where(u => u.Email == email).Select(u => u.Password).SingleOrDefault();
            if (existingEmail != 0)
            {
                if (password == existingPassword)
                {
                    var employerFound = _context.Users.OfType<Employer>().Count(a => a.Email == email);

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

        public List<User> GetAllUsers()
        {
            var query = from b in _context.Users
                        orderby b.Email
                        select b;

            return query.ToList();
        }
    }

    public class JobCompanyTests
    {
       
        [Fact]
        public void VacancyCreationTest()
        {
            var connection = Effort.DbConnectionFactory.CreateTransient();

            using (var context = new SampleDbContext(connection))
            {
                 var list = new List<User>
                 { 
                    new Employer { Email = "1@mail.ru", Password = "123456" , Company = "ALMA"},
                    new Employer { Email = "2@mail.ru", Password = "qwerty", Company = "LAMA" }
                 }.AsQueryable();

                context.Users.AddRange(list);
                context.SaveChanges();
            }

            using (var context = new SampleDbContext(connection))
            {
                var user = context.Users.Where(x => x.Email == "1@mail.ru").FirstOrDefault();

                var list = new List<Vacancy>
                {   new Vacancy { Employer = (Employer)user, Name = "First Job", Salary = 35000, Type = "Simple Job", Requirements = "Straightforward" },
                    new Vacancy { Employer = (Employer)user, Name = "Second Job", Salary = 45000, Type = "Difficult Job", Requirements = "Straightforward" }
                }.AsQueryable();

                context.Vacancies.AddRange(list);
                context.SaveChanges();
            }

            using (var context = new SampleDbContext(connection))
            {
                var vacancies = context.Vacancies.ToList();
                Assert.Equal("First Job", vacancies.ElementAt(0).Name);
                Assert.Equal("Second Job", vacancies.ElementAt(1).Name);
            }
        }

        [Fact]
        public void VacancyResponseTest()
        {
            var connection = Effort.DbConnectionFactory.CreateTransient();

            using (var context = new SampleDbContext(connection))
            {
                var list = new List<User>
                 {
                    new Employee { Email = "1@mail.ru", Password = "123456" , Name = "John Smith", Portfolio = "Just Ordinary Guy"},
                    new Employer { Email = "2@mail.ru", Password = "qwerty", Company = "LAMA" }
                 }.AsQueryable();

                context.Users.AddRange(list);
                context.SaveChanges();
            }

            using (var context = new SampleDbContext(connection))
            {
                var user = context.Users.Where(x => x.Email == "2@mail.ru").FirstOrDefault();

                var list = new List<Vacancy>
                {   new Vacancy { Employer = (Employer)user, Name = "First Job", Salary = 35000, Type = "Simple Job", Requirements = "Straightforward" },
                    new Vacancy { Employer = (Employer)user, Name = "Second Job", Salary = 45000, Type = "Difficult Job", Requirements = "Straightforward" }
                }.AsQueryable();

                context.Vacancies.AddRange(list);
                context.SaveChanges();
            }

            using (var context = new SampleDbContext(connection))
            {
                var vacancy = context.Vacancies.Where(x => x.Name == "First Job").FirstOrDefault();

                var user = context.Users.Where(x => x.Email == "1@mail.ru").FirstOrDefault();

                vacancy.EmployeesResponses.Add((Employee)user);
                context.SaveChanges();
            }

            using (var context = new SampleDbContext(connection))
            {
                var vacancy = context.Vacancies
               .Include(x => x.EmployeesResponses)
               .Where(x => x.Name == "First Job")
               .FirstOrDefault();

                var responses = vacancy.EmployeesResponses.ToList();

                Assert.Equal("1@mail.ru", responses.ElementAt(0).Email);
            }
        }
    }
}

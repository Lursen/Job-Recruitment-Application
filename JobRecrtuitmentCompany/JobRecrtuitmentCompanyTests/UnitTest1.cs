using JobRecrtuitmentCompany;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;



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

    public class LoginTest
    {
        [Fact]
        public void Test1()
        {
            var data = new List<User>
            {
                new Employer { Email = "1@mail.ru", Password = "123456" , Company = "ALMA"},
                new Employee { Email = "2@mail.ru", Password = "qwerty", Name = "John Smith" }
            }.AsQueryable();


            var mockSet = new Mock<System.Data.Entity.DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SampleDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UserService(mockContext.Object);
            var users = service.GetAllUsers();

            Assert.Equal(2, users.Count);
            Assert.Equal("1@mail.ru", users[0].Email);
            Assert.Equal("2@mail.ru", users[1].Email);

            Assert.Equal(1, service.LogIn("1@mail.ru", "123456"));
            Assert.Equal(2, service.LogIn("2@mail.ru", "qwerty"));
        }
    }
}

using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.EntityFramework;
using System;
using System.Linq;

namespace Imagine.BookManager.Migrations
{
    public class InitDbContext
    {
        private readonly BookManagerDbContext _dbContext;

        public InitDbContext(BookManagerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.CreateIfNotExists();

        }

        public void Create()
        {
            CreateInstitution();
            CreateAdmin();
            CreateClass();
        }

        public void CreateAdmin()
        {
            Institution institution = _dbContext.Institution.First();

            Admin admin = new Admin()
            {
                DateCreated = DateTime.Now,
                Email = "brian@imaginelearning.cn",
                FullName = "brian",
                Gender = true,
                InstitutionId = institution.Id,
                IsDelete = false,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "brian",
                UserType = UserType.Admin
            };
            Admin admin2 = new Admin()
            {
                DateCreated = DateTime.Now,
                Email = "brian@imaginelearning.cn",
                FullName = "brian",
                Gender = true,
                InstitutionId = institution.Id,
                IsDelete = false,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "empty",
                UserType = UserType.Admin
            };
            _dbContext.Admin.Add(admin);
            _dbContext.Admin.Add(admin2);
            _dbContext.SaveChanges();
        }

        public void CreateInstitution()
        {
            Institution institution = new Institution()
            {
                Address = "上海",
                District = "浦东新区",
                Name = "FirstInstitution",
                Tel = "18817617807"
            };
            _dbContext.Institution.Add(institution);
            _dbContext.SaveChanges();
        }

        public void CreateClass()
        {
            Institution institution = _dbContext.Institution.First();
            ClassInfo classInfo = new ClassInfo()
            {
                InstitutionId = institution.Id,
                Name = "Class1",
                ReminderInterva = 180,
                DateCreated = DateTime.Now
            };
            _dbContext.ClassInfo.Add(classInfo);
            _dbContext.SaveChanges();
        }
    }
}

using Abp.UI;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.StudentService;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class StudentAppServiceTest : BookManagerTestBase
    {
        private readonly IStudentAppService _studentAppService;

        public StudentAppServiceTest()
        {
            _studentAppService = Resolve<IStudentAppService>();
        }

        [Fact]
        public void CreateStudent_Return_StudentId_If_Success()
        {
            Student student = new Student
            {
                UserName = "brian",
                DateCreated = DateTime.Now,
                FullName = "12345",
                Gender = true,
                GuardianName = "Brian",
                IsDelete = false,
                Mobile = "1234567",
                Password = "123456",
                Picture = "111111111"
            };
            var result = _studentAppService.CreateStudent(student);
            Assert.True(result > 0);
        }

        [Fact]
        public void CreateStudent_Throw_Exception_If_UserName_Exists()
        {
            Student student = InitFakeEntity.GetFakeStudent();
            UsingDbContext(ctx => ctx.Student.Add(student));
            Student student2 = InitFakeEntity.GetFakeStudent();
            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() => _studentAppService.CreateStudent(student2));
            Assert.True(ex != null);
        }

        [Fact]
        public void GetAllStudentList_Should_Return_Correct_Number_Of_Records()
        {
            //Arrange:Insert Data
            for (int i = 0; i < 10; i++)
            {
                Student student = InitFakeEntity.GetFakeStudent();
                student.UserName = "brian" + i;
                UsingDbContext(ctx => ctx.Student.Add(student));
            }
            var result = _studentAppService.GetAllStudentList(1);
            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void GetAllStudentList_Return_Empty()
        {
            var result = _studentAppService.GetAllStudentList(1);
            result.CurrentPage.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
        }

        [Fact]
        public void UpdateStudent_Return_True_If_Success()
        {
            string fullName = "654321";
            Student student = InitFakeEntity.GetFakeStudent();
            var student2 = UsingDbContext(ctx => ctx.Student.Add(student));
            student2.FullName = fullName;
            _studentAppService.UpdateStudent(student2);
            var student3 = UsingDbContext(ctx => ctx.Student.First());
            student3.FullName.ShouldBe(fullName);
        }

        [Fact]
        public void UpdateStudent_Throw_Exception_If_Student_Not_Found()
        {
            Student student = InitFakeEntity.GetFakeStudent();
            var ex = Should.Throw<UserFriendlyException>(() => _studentAppService.UpdateStudent(student));
            Assert.True(ex != null);
        }

        [Fact]
        public void CheckStudentName_Return_True_If_Not_Found()
        {
            var result = _studentAppService.CheckStudentName("8765");
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckStudentName_Return_False_If__Found()
        {
            Student student = InitFakeEntity.GetFakeStudent();
            var student2 = UsingDbContext(ctx => ctx.Student.Add(student));
            var result = _studentAppService.CheckStudentName(student2.UserName);
            result.ShouldBe(false);
        }
    }
}
using Abp.UI;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.StudentService;
using Shouldly;
using System;
using System.Collections.Generic;
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

        #region SearchStudent
        private Guid setSearchStudentData()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminDto = InitFakeEntity.GetFakeAdmin();
            adminDto.UserId = Guid.NewGuid();
            adminDto.UserType = UserType.Admin;
            adminDto.InstitutionId = institution.Id;

            var classDto = InitFakeEntity.GetFakeClassInfo();
            classDto.Admins.Add(adminDto);
            classDto.InstitutionId = institution.Id;

            for (int i = 0; i < 3; i++)
            {
                var studentDto = InitFakeEntity.GetFakeStudent();
                studentDto.StudentId = Guid.NewGuid();
                studentDto.UserName = studentDto.UserName + i;
                classDto.Students.Add(studentDto);
            }
            var classInfo = UsingDbContext(e => e.ClassInfo.Add(classDto));

            for (int i = 0; i < 3; i++)
            {
                var orderDto = InitFakeEntity.GetFakeOrder();
                orderDto.UserId = classInfo.Admins.First().UserId;
                orderDto.OrderRef = orderDto.OrderRef + i;
                orderDto.Paid = true;
                var order = UsingDbContext(e => e.Order.Add(orderDto));

                var setDto = InitFakeEntity.GetFakeSet();
                setDto.SetName = setDto.SetName + i;
                var set = UsingDbContext(e => e.Sets.Add(setDto));

                var orderItemDto = InitFakeEntity.GetFakeOrderItem();
                orderItemDto.SetId = set.Id;
                orderItemDto.OrderRef = order.OrderRef;
                orderItemDto.UserId = order.UserId;
                var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemDto));

                var adminDto2 = InitFakeEntity.GetFakeAdmin();
                adminDto2.UserName = adminDto2.UserName + i;
                adminDto2.UserId = Guid.NewGuid();
                adminDto2.InstitutionId = institution.Id;

                var classDto2 = InitFakeEntity.GetFakeClassInfo();
                classDto2.Admins.Add(adminDto2);
                classDto2.InstitutionId = institution.Id;
                classDto2.Name = classDto2.Name + i;

                for (int k = 0; k < 3; k++)
                {
                    var studentDto = InitFakeEntity.GetFakeStudent();
                    studentDto.StudentId = Guid.NewGuid();
                    studentDto.UserName = studentDto.UserName + i + k;
                    classDto2.Students.Add(studentDto);
                }
                var classInfo2 = UsingDbContext(e => e.ClassInfo.Add(classDto2));

                var teacherAllocationDto = InitFakeEntity.GetFakeTeacherAllocation();
                teacherAllocationDto.OrderItemId = orderItem.Id;
                teacherAllocationDto.SetId = set.Id;
                teacherAllocationDto.TeacherId = classInfo2.Admins.First().UserId;
                var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationDto));


                UsingDbContext(e =>
                {
                    foreach (var item in classInfo2.Students)
                    {
                        var studentAllocationDto = new StudentAllocation();
                        studentAllocationDto.TeacherAllocationId = teacherAllocation.Id;
                        studentAllocationDto.StudentId = item.StudentId;
                        e.StudentAllocation.Add(studentAllocationDto);
                    }

                });

            }
            return classInfo.Admins.First().UserId;
        }


        [Fact]
        public void SearchStudent_Return_True_If_NoConditions()
        {
            Guid userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "", null, 0, null, "", null, userId);
            list.ListData.Count.ShouldBe(10);
            list.CurrentPage.ShouldBe(1);
            list.TotalPages.ShouldBe(2);
        }

        [Fact]
        public void SearchStudent_Return_True_If_Admin_NotExists()
        {
            var list = _studentAppService.SearchStudent(1, 10, "", null, 0, null, "", null, Guid.NewGuid());
            list.ListData.Count.ShouldBe(0);
            list.CurrentPage.ShouldBe(0);
            list.TotalPages.ShouldBe(0);
        }

        [Fact]
        public void SearchStudent_Return_True_If_ClassId_Exists()
        {
            var userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "", 3, 0, null, "", null, userId);
            list.ListData.Count.ShouldBe(3);
            list.CurrentPage.ShouldBe(1);
            list.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchStudent_Return_True_If_StudentName_Mobile_DateCreated()
        {
            var userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "2", 0, 0, null, "1234567", DateTime.Now, userId);
            list.ListData.Count.ShouldBe(6);
            list.CurrentPage.ShouldBe(1);
            list.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchStudent_Return_True_If_SetId_Exists()
        {
            var userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "", 0, 0, 3, "", null, userId);
            list.ListData.Count.ShouldBe(3);
            list.CurrentPage.ShouldBe(1);
            list.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchStudent_Return_True_If_SetStatus_Allocated()
        {
            var userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "", 0, 1, null, "", null, userId);
            list.ListData.Count.ShouldBe(9);
            list.CurrentPage.ShouldBe(1);
            list.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchStudent_Return_True_If_SetStatus_UnAllocated()
        {
            var userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "", 0, 2, null, "", null, userId);
            list.ListData.Count.ShouldBe(3);
            list.CurrentPage.ShouldBe(1);
            list.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchStudent_Return_True_If_SetStatus_Expire()
        {
            var userId = setSearchStudentData();
            var list = _studentAppService.SearchStudent(1, 10, "", 0, 3, null, "", null, userId);
            list.ListData.Count.ShouldBe(0);
            list.CurrentPage.ShouldBe(0);
            list.TotalPages.ShouldBe(0);
        }
        #endregion
    }
}
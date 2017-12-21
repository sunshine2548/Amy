using Abp.UI;
using Imagine.BookManager.ClassService;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Class;
using Shouldly;
using System;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class ClassAppServiceTest : BookManagerTestBase
    {
        private readonly IClassAppService _classAppService;

        public ClassAppServiceTest()
        {
            _classAppService = LocalIocManager.Resolve<IClassAppService>();
        }

        #region Create Class
        [Fact]
        public void CreateClassInfo_Success()
        {
            Admin admin = UsingDbContext(ctx => ctx.Admin.First());
            ClassInfoIn classInfo = new ClassInfoIn
            {
                Name = "Brian2",
                ReminderInterva = 180,
                UserId = admin.UserId
            };
            var result = _classAppService.CreateClassInfo(classInfo);
            result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void CreateClassInfo_Return_0_If_The_UserId_Not_Exist()
        {
            ClassInfoIn classInfo = new ClassInfoIn
            {
                Name = "Brian",
                ReminderInterva = 180,
                UserId = new Guid()
            };
            var result = _classAppService.CreateClassInfo(classInfo);
            result.ShouldBe(0);
        }

        [Fact]
        public void CreateClassInfo_Return_0_If_The_User_Not_Fount_Institution()
        {

            Admin admin = UsingDbContext(ctx => ctx.Admin.Add(InitFakeEntity.GetFakeAdmin()));
            ClassInfoIn classInfo = new ClassInfoIn
            {
                Name = "Brian",
                ReminderInterva = 180,
                UserId = admin.UserId
            };
            var result = _classAppService.CreateClassInfo(classInfo);
            result.ShouldBe(0);
        }
        #endregion

        #region AllocationClassInfoToTeacher
        [Fact]
        public void AllocationClassInfoToTeacher_Return_True_If_Success()
        {
            Institution institution = UsingDbContext(ctx => ctx.Institution.First());
            Admin admin = InitFakeEntity.GetFakeAdmin();
            admin.InstitutionId = institution.Id;
            Admin adminResult = UsingDbContext(c => c.Admin.Add(admin));
            ClassInfo classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            bool b = _classAppService.AllocationClassInfoToTeacher(classInfo.Id, adminResult.UserId);
            b.ShouldBe(true);
        }

        [Fact]
        public void AllocationClassInfoToTeacher_Return_False_If_User_Not_Exists()
        {
            ClassInfo classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            bool b = _classAppService.AllocationClassInfoToTeacher(classInfo.Id, new Guid());
            b.ShouldBe(false);
        }

        [Fact]
        public void AllocationClassInfoToTeacher_Return_False_If_Class_Not_Exists()
        {
            Institution institution = UsingDbContext(ctx => ctx.Institution.First());
            Admin admin = InitFakeEntity.GetFakeAdmin();
            admin.InstitutionId = institution.Id;
            Admin adminResult = UsingDbContext(c => c.Admin.Add(admin));
            bool b = _classAppService.AllocationClassInfoToTeacher(100, adminResult.UserId);
            b.ShouldBe(false);
        }

        [Fact]
        public void AllocationClassInfoToTeacher_Return_False_If_User_Already_Has_The_Class()
        {
            var instituionTemp = InitFakeEntity.GetFakeInstitution();
            var institution = UsingDbContext(ctx => ctx.Institution.Add(instituionTemp));
            Admin admin = InitFakeEntity.GetFakeAdmin();
            admin.UserType = UserType.Teacher;
            admin.InstitutionId = institution.Id;

            ClassInfo classInfoTemp = new ClassInfo
            {
                Name = "Class2111",
                ReminderInterva = 180,
                InstitutionId = institution.Id,
                DateCreated = DateTime.Now
            };

            UsingDbContext(ctx =>
            {
                ctx.Admin.Add(admin);
                classInfoTemp.Admins.Add(admin);
                ctx.ClassInfo.Add(classInfoTemp);
            });
            var adminResult = UsingDbContext(ctx => ctx.Admin.Include(x => x.Classes).Single(x => x.UserName == admin.UserName));
            var classInfo = UsingDbContext(ctx => ctx.ClassInfo.Include(x => x.Admins).Single(x => x.Name == classInfoTemp.Name));
            bool b = _classAppService.AllocationClassInfoToTeacher(classInfo.Id, adminResult.UserId);
            b.ShouldBe(false);
        }

        [Fact]
        public void AllocationClassInfoToTeacher_Return_False_If_User_Institution_Not_Equals_Class()
        {
            Institution institution = InitFakeEntity.GetFakeInstitution();
            var institution2 = UsingDbContext(ctx => ctx.Institution.Add(institution));
            Admin admin = InitFakeEntity.GetFakeAdmin();
            admin.InstitutionId = institution2.Id;
            admin.UserType = UserType.Teacher;

            Admin adminResult = UsingDbContext(c => c.Admin.Add(admin));
            ClassInfo classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            bool b = _classAppService.AllocationClassInfoToTeacher(classInfo.Id, adminResult.UserId);
            b.ShouldBe(false);
        }
        #endregion

        #region GetInstitutionAllClasses
        [Fact]
        public void GetInstitutionAllClasses_Return_Empty_If_Institution_Not_Exists()
        {
            PaginationDataList<ClassInfoOut> result = _classAppService.GetInstitutionAllClasses(-1, 1);
            result.CurrentPage.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }

        [Fact]
        public void GetInstitutionAllClasses_Return_10_Data()
        {
            Institution institution = InitFakeEntity.GetFakeInstitution();
            for (var i = 0; i < 10; i++)
            {
                ClassInfo classInfo = new ClassInfo
                {
                    Name = "Class2",
                    ReminderInterva = 180,
                    DateCreated = DateTime.Now
                };
                institution.ClassInfos.Add(classInfo);
            }
            var institution2 = UsingDbContext(ctx => ctx.Institution.Add(institution));
            PaginationDataList<ClassInfoOut> result = _classAppService.GetInstitutionAllClasses(institution2.Id, 1);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }
        #endregion

        #region DeleteUsersClassInfo
        [Fact]
        public void DeleteUsersClassInfo_Return_True_If_Success()
        {
            var instituionTemp = InitFakeEntity.GetFakeInstitution();
            var institution = UsingDbContext(ctx => ctx.Institution.Add(instituionTemp));

           

            Admin admin = InitFakeEntity.GetFakeAdmin();
            admin.InstitutionId = institution.Id;
            admin.UserType = UserType.Teacher;

            ClassInfo classInfoTemp = new ClassInfo
            {
                Name = "Class2111",
                ReminderInterva = 180,
                InstitutionId = institution.Id,
                DateCreated = DateTime.Now
            };

            UsingDbContext(ctx =>
            {
                ctx.Admin.Add(admin);
                classInfoTemp.Admins.Add(admin);
                ctx.ClassInfo.Add(classInfoTemp);
            });
            var adminResult = UsingDbContext(ctx => ctx.Admin.Include(x => x.Classes).Single(x => x.UserName == admin.UserName));
            var classInfo2 = UsingDbContext(ctx => ctx.ClassInfo.Include(x => x.Admins).Single(x => x.Name == classInfoTemp.Name));
            var result = _classAppService.DeleteUsersClassInfo(classInfo2.Id, adminResult.UserId);
            result.ShouldBe(true);
        }

        [Fact]
        public void DeleteUsersClassInfo_Return_False_If_User_Not_Exists()
        {
            ClassInfo classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            var result = _classAppService.DeleteUsersClassInfo(classInfo.Id, new Guid());
            result.ShouldBe(false);
        }

        [Fact]
        public void DeleteUsersClassInfo_Return_False_If_Class_Not_Exists()
        {
            Admin admin = UsingDbContext(ctx => ctx.Admin.First());
            var result = _classAppService.DeleteUsersClassInfo(-1, admin.UserId);
            result.ShouldBe(false);
        }

        [Fact]
        public void DeleteUsersClassInfo_Return_False_If_User_ClassId_Not_Contains_ClassId()
        {
            Admin admin = UsingDbContext(ctx => ctx.Admin.First());
            Institution institution = InitFakeEntity.GetFakeInstitution();
            var institution2 = UsingDbContext(ctx => ctx.Institution.Add(institution));
            ClassInfo classInfo = new ClassInfo
            {
                InstitutionId = institution2.Id,
                Name = "Class1",
                ReminderInterva = 180,
                DateCreated = DateTime.Now
            };
            ClassInfo classInfo2 = UsingDbContext(ctx => ctx.ClassInfo.Add(classInfo));
            var result = _classAppService.DeleteUsersClassInfo(classInfo2.Id, admin.UserId);
            result.ShouldBe(false);
        }
        #endregion

        #region GetClassInfoById
        [Fact]
        public void GetClassInfoById_Return_Entity_If_Success()
        {
            var classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            var getClass = _classAppService.GetClassInfoById(classInfo.Id);
            getClass.Name.ShouldBe(classInfo.Name);
            true.ShouldBe(getClass.Id > 0);
        }

        [Fact]
        public void GetClassInfoById_Return_Null_If_Success()
        {
            var getClass = _classAppService.GetClassInfoById(-1);
            getClass.ShouldBe(null);
        }
        #endregion

        #region GetClassInfoTeachers
        [Fact]
        public void GetClassInfoTeachers__Should_Return_Correct_Number_Of_Records()
        {
            Institution institution = UsingDbContext(ctx => ctx.Institution.First());
            ClassInfo classInfo = new ClassInfo
            {
                Name = "Class2",
                ReminderInterva = 180,
                InstitutionId = institution.Id,
                DateCreated = DateTime.Now
            };
            for (int i = 0; i < 10; i++)
            {
                Admin admin=  InitFakeEntity.GetFakeAdmin();
                admin.UserName = "testClass" + i;
                classInfo.Admins.Add(admin);
            }
            ClassInfo classInfo2 = UsingDbContext(c => c.ClassInfo.Add(classInfo));

            var result = _classAppService.GetClassInfoTeachers(classInfo2.Id, 1);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void GetClassInfoTeachers_Should_Return_Correct_Number_Of_Records_And_SingletonPage_Is_Not_Null()
        {
            Institution institution = UsingDbContext(ctx => ctx.Institution.First());

            ClassInfo classInfo = new ClassInfo
            {
                Name = "Class2",
                ReminderInterva = 180,
                InstitutionId = institution.Id,
                DateCreated = DateTime.Now
            };
            for (int i = 0; i < 10; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = "testClass" + i;
                classInfo.Admins.Add(admin);
            }
            ClassInfo classInfo2 = UsingDbContext(c => c.ClassInfo.Add(classInfo));

            var result = _classAppService.GetClassInfoTeachers(classInfo2.Id, 1, 10);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void GetClassInfoTeachers_Return_Default_Data_If_Not_Exists()
        {
            Institution institution = UsingDbContext(ctx => ctx.Institution.First());

            ClassInfo classInfo = new ClassInfo
            {
                Name = "Class3",
                ReminderInterva = 180,
                InstitutionId = institution.Id,
                DateCreated = DateTime.Now
            };
            ClassInfo classInfo2 = UsingDbContext(c => c.ClassInfo.Add(classInfo));
            var result = _classAppService.GetClassInfoTeachers(classInfo2.Id, 1);
            result.CurrentPage.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }
        #endregion

        #region AllocationStudentClass
        [Fact]
        public void AllocationStudentClass_Return_True_If_Success()
        {
            Student student = InitFakeEntity.GetFakeStudent();
            var student2 = UsingDbContext(ctx => ctx.Student.Add(student));
            ClassInfo classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            var result = _classAppService.AllocationStudentClass(student2.StudentId, classInfo.Id);
            result.ShouldBe(true);
        }

        [Fact]
        public void AllocationStudentClass_Throw_Exception_If_Student_Not_Exists()
        {
            ClassInfo classInfo = UsingDbContext(ctx => ctx.ClassInfo.First());
            Should.Throw<UserFriendlyException>(() =>
            {
                _classAppService.AllocationStudentClass(new Guid(), classInfo.Id);
            });
        }

        [Fact]
        public void AllocationStudentClass_Throw_Exception_If_Class_Not_Exists()
        {
            Student student = InitFakeEntity.GetFakeStudent();
            var student2 = UsingDbContext(ctx => ctx.Student.Add(student));
            Should.Throw<UserFriendlyException>(() =>
            {
                _classAppService.AllocationStudentClass(student2.StudentId, -1);
            });
        }
        #endregion
    }
}
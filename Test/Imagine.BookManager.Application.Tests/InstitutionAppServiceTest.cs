using Abp.UI;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.InstitutionService;
using Shouldly;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using Imagine.BookManager.Dto.Institution;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class InstitutionAppServiceTest : BookManagerTestBase
    {
        private readonly IInstitutionAppService _institutionAppService;

        public InstitutionAppServiceTest()
        {
            _institutionAppService = LocalIocManager.Resolve<IInstitutionAppService>();
        }

        #region CreateInstitution Sync
        [Fact]
        public void CreateInstitution_Should_Success()
        {
            //Arrange
            var count = UsingDbContext(ctx => ctx.Institution.Count());

            _institutionAppService.CreateInstitution(InitFakeEntity.GetFakeInstitution());
            //Act
            var count2 = UsingDbContext(ctx => ctx.Institution.Count());
            //Assert
            count.ShouldBe(count2 - 1);
        }

        [Fact]
        public void CreateInstitution_Should_Error_If_Name_Exist()
        {
            var institution = InitFakeEntity.GetFakeInstitution();
            UsingDbContext(ctx => ctx.Institution.Add(institution));
            var ex = Assert.Throws<UserFriendlyException>(
                () => _institutionAppService.CreateInstitution(InitFakeEntity.GetFakeInstitution()));
            Assert.True(ex != null);

        }
        #endregion

        #region Create institution Async
        [Fact]
        public void CreateInstitutionAsync_Should_Success()
        {
            var institution = InitFakeEntity.GetFakeInstitution();
            var result = _institutionAppService.CreateInstitutionAsync(institution);
            Assert.True(result.Result > 0);
        }

        [Fact]
        public async void CreateInstitutionAsync_Should_Error_If_Name_Exist()
        {
            var institution = InitFakeEntity.GetFakeInstitution();
            UsingDbContext(ctx => ctx.Institution.Add(institution));
            UserFriendlyException ex = await Assert.ThrowsAsync<UserFriendlyException>(
                    () => _institutionAppService.CreateInstitutionAsync(institution)
                    );
            Assert.True(ex != null);
        }
        #endregion

        #region GetInstitutionById Sync

        [Fact]
        public void GetInstitution_Should_Return_Entity_If_Found()
        {
            var institution = UsingDbContext(context => context.Institution.First());

            var result = _institutionAppService.GetInstitution(institution.Id);

            result.Id.ShouldBe(institution.Id);
        }

        [Fact]
        public void GetInstitution_Should_Return_Null_If_Not_Found()
        {
            var id = -1000;
            var result = _institutionAppService.GetInstitution(id);
            result.ShouldBe(null);
        }
        #endregion

        #region GetInstitution Async
        [Fact]
        public async void GetInstitutionAysnc_Should_Return_Entity_If_Found()
        {
            var institution = UsingDbContext(context => context.Institution.First());

            var result = await _institutionAppService.GetInstitutionAsync(institution.Id);

            result.Id.ShouldBe(institution.Id);
        }

        [Fact]
        public async void GetInstitutionAsync_Should_Return_Null_If_Not_Found()
        {
            var id = 1000;
            var result = await _institutionAppService.GetInstitutionAsync(id);
            result.ShouldBe(null);
        }
        #endregion

        #region GetInstitutionByName

        [Fact]
        public void GetInstitutionByName_Should_Return_Entity_If_Found()
        {
            string name = "FirstInstitution";
            var result = _institutionAppService.GetInstitutionByName(name);
            name.ShouldBe(result.Name);
        }

        [Fact]
        public void GetInstitutionByName_Should_Return_Empty_If_Not_Found()
        {
            string name = "FirstInstitution11";
            var result = _institutionAppService.GetInstitutionByName(name);
            result.Id.ShouldBe(0);
        }
        #endregion

        #region CheckInstitutionName
        [Fact]
        public void CheckInstitutionName_Should_Return_False_If_Not_Found()
        {
            string name = "FirstInstitution";
            var result = _institutionAppService.CheckInstitutionName(name);
            result.ShouldBe(false);
        }

        [Fact]
        public void CheckInstitutionName_Should_Return_True_If_Found()
        {
            string name = "FirstInstitution1";
            var result = _institutionAppService.CheckInstitutionName(name);
            result.ShouldBe(true);
        }
        #endregion

        #region GetAll
        [Fact]
        public void GetAll_PageIndex_And_SingletonPageCount_Is_Null()
        {
            for (var i = 0; i < 15; i++)
            {
                var institution = InitFakeEntity.GetFakeInstitution();
                institution.Name = institution.Name + i;
                UsingDbContext(context => context.Institution.Add(institution));
            }
            int totalPages = 0;
            string strsingleton = ConfigurationManager.AppSettings["SingletonPageCount"];
            double.TryParse(strsingleton, out var singletonCount);
            UsingDbContext(context =>
            {
                totalPages = (int)(Math.Ceiling(context.Institution.Count() / singletonCount));
            });
            var result = _institutionAppService.GetAll(null);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(totalPages);
        }

        [Fact]
        public void GetAll_PageIndex_And_SingletonPageCount_Is_Not_Null()
        {
            for (var i = 0; i < 15; i++)
            {
                var institution = InitFakeEntity.GetFakeInstitution();
                institution.Name = institution.Name + i;
                UsingDbContext(context => context.Institution.Add(institution));
            }
            int totalPages = 0;
            string strsingleton = ConfigurationManager.AppSettings["SingletonPageCount"];
            double.TryParse(strsingleton, out var singletonCount);
            UsingDbContext(context =>
            {
                totalPages = (int)(Math.Ceiling(context.Institution.Count() / singletonCount));
            });
            var result = _institutionAppService.GetAll(1, 10);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(totalPages);
        }
        #endregion

        #region GetAllInstitutionIdStudent
        [Fact]
        public void GetAllInstitutionIdStudent_Should_Return_Correct_Number_Of_Records()
        {
            #region Arrange Insert data
            var instituion = InitFakeEntity.GetFakeInstitution();
            Admin admin = InitFakeEntity.GetFakeAdmin();
            admin.UserName = "abcdefg";
            Admin admin2 = InitFakeEntity.GetFakeAdmin();
            admin2.UserName = "aaaaaaa";
            instituion.Admins.Add(admin);
            instituion.Admins.Add(admin2);
            Random random = new Random();
            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                ClassInfo classInfo = new ClassInfo
                {
                    Name = "ClassTest" + i,
                    ReminderInterva = 180 + i,
                    DateCreated = DateTime.Now
                };
                int len = random.Next(5, 10);
                count += len;
                for (int j = 0; j < len; j++)
                {
                    Student student = InitFakeEntity.GetFakeStudent();
                    student.UserName = "brian" + i + "-" + j;
                    classInfo.Students.Add(student);
                }
                classInfo.Admins.Add(i % 2 == 0 ? admin : admin2);
                instituion.ClassInfos.Add(classInfo);
            }
            var institution2 = UsingDbContext(ctx => ctx.Institution.Add(instituion));
            #endregion
            var result = _institutionAppService.GetAllInstitutionIdStudent(institution2.Id, 1, count);
            result.TotalPages.ShouldBe(1);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(count);
        }

        [Fact]
        public void GetAllInstitutionIdStudent_Return_Empty()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.First());
            var result = _institutionAppService.GetAllInstitutionIdStudent(institution.Id, 1);
            result.TotalPages.ShouldBe(0);
            result.CurrentPage.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
        }
        #endregion

        [Fact]
        public void UpdateInstitution()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            InstitutionOut getInstitution = new InstitutionOut()
            {
                Id = institution.Id,
                District = "abssssss",
                Name = "brian_test",
                Address = "shanghai",
                Tel = "1"
            };

            var result = _institutionAppService.Update(getInstitution);

            var resultInstitution = UsingDbContext(
                ctx => ctx.Institution.First(x => x.Id == institution.Id
                ));

            result.ShouldBe(true);

            resultInstitution.District.ShouldBe(getInstitution.District);
            resultInstitution.Name.ShouldBe(getInstitution.Name);
            resultInstitution.Address.ShouldBe(getInstitution.Address);
        }


        [Fact]
        public void UpdateInstitution_Return_True_With_Admins()
        {
            var institutionTemp = InitFakeEntity.GetFakeInstitution();
            var admin = InitFakeEntity.GetFakeAdmin();

            institutionTemp.Admins.Add(admin);

            var institution = UsingDbContext(ctx => ctx.Institution.Add(institutionTemp));
            InstitutionOut getInstitution = new InstitutionOut()
            {
                Id = institution.Id,
                District = "abssssss",
                Name = "brian_test",
                Address = "shanghai",
                Tel = "1"
            };

            var result = _institutionAppService.Update(getInstitution);

            var resultInstitution = UsingDbContext(
                ctx => ctx.Institution.Include(x => x.Admins).First(x => x.Id == institution.Id
                ));


            result.ShouldBe(true);
            resultInstitution.District.ShouldBe(getInstitution.District);
            resultInstitution.Name.ShouldBe(getInstitution.Name);
            resultInstitution.Address.ShouldBe(getInstitution.Address);
            resultInstitution.Admins.Count.ShouldBe(1);
        }
    }
}

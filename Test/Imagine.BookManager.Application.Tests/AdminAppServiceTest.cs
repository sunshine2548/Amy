using Abp.UI;
using Imagine.BookManager.AdminService;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Admin;
using Shouldly;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class AdminAppServiceTest : BookManagerTestBase
    {
        private readonly IAdminAppService _iAdminAppSerice;

        public AdminAppServiceTest()
        {
            _iAdminAppSerice = Resolve<IAdminAppService>();
        }
        
        #region CreateAdmin sync
        [Fact]
        public void CreateAdmin_Return_True_If_Success_Without_InstitutionId()
        {
            AdminDto admin = InitFakeEntity.GetFakeAdminDto();
            var result = _iAdminAppSerice.CreateAdmin(admin);
            (result > 0).ShouldBe(true);
        }

        [Fact]
        public void CreateAdmin_Return_True_If_Success_And_Institution_Exists()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.First());
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "brian2",
                UserType = (int)UserType.Admin,
                InstitutionId = institution.Id
            };
            var result = _iAdminAppSerice.CreateAdmin(admin);
            (result > 0).ShouldBe(true);
        }

        [Fact]
        public void CreateAdmin_Throw_Exception_If_Institution_Not_Exists()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "brian2",
                UserType = (int)UserType.Admin,
                InstitutionId = 3
            };
            var ex = Should.Throw<UserFriendlyException>(() =>
              {
                  _iAdminAppSerice.CreateAdmin(admin);
              });
            Assert.True(ex != null);
        }

        [Fact]
        public void CreateAdmin_Throw_Exception_If_UserName_Already_Exists()
        {
            var admin2 = UsingDbContext(ctx => ctx.Admin.First());
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = admin2.UserName,
                UserType = (int)UserType.Admin
            };
            var ex = Should.Throw<UserFriendlyException>(() =>
              {
                  _iAdminAppSerice.CreateAdmin(admin);
              });
            Assert.True(ex != null);
        }

        [Fact]
        public void CreateAdmin_Throw_Exception_If_UserName_Is_Null()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserType = (int)UserType.Admin
            };
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(() => _iAdminAppSerice.CreateAdmin(admin));
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public void CreateAdmin_Throw_Exception_If_Password_Is_Null()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                UserName = "123456",
                UserType = (int)UserType.Admin
            };
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(() => _iAdminAppSerice.CreateAdmin(admin));
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public void CreateAdmin_Throw_Exception_If_UserName_Length_Greater_Then_30()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 31; i++)
            {
                sb.Append(i);
            }
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                UserName = sb.ToString(),
                UserType = (int)UserType.Admin,
                Password = "123456"
            };
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(() => _iAdminAppSerice.CreateAdmin(admin));
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public void CreateAdmin_Throw_Exception_If_Password_Length_Greater_Then_23()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 24; i++)
            {
                sb.Append(i);
            }
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                UserName = "1234",
                UserType = (int)UserType.Admin,
                Password = sb.ToString()
            };
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(() => _iAdminAppSerice.CreateAdmin(admin));
            (ex != null).ShouldBe(true);
        }
        #endregion

        #region CreateAdminAsync
        [Fact]
        public async void CreateAdminAsync_Return_True_If_Success_Without_InstitutionId()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "brian2",
                UserType = (int)UserType.Admin
            };
            var result = await _iAdminAppSerice.CreateAdminAsync(admin);
            (result > 0).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Return_Ture_If_Sucess_With_InstitutionId()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.First());
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "brian2",
                UserType = (int)UserType.Admin,
                InstitutionId = institution.Id
            };
            var result = await _iAdminAppSerice.CreateAdminAsync(admin);
            (result > 0).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Throw_Exception_If_UserName_Already_Exist()
        {
            Admin adminTemp = UsingDbContext(ctx => ctx.Admin.First());
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = adminTemp.UserName,
                UserType = (int)UserType.Admin
            };
            UserFriendlyException ex = await Assert
                .ThrowsAsync<UserFriendlyException>(
                    () => _iAdminAppSerice.CreateAdminAsync(admin)
                );
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Throw_Exception_If_InstitutionId_Not_Exists()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "123",
                InstitutionId = 100,
                UserType = (int)UserType.Admin
            };
            UserFriendlyException ex = await Assert
                .ThrowsAsync<UserFriendlyException>(
                    () => _iAdminAppSerice.CreateAdminAsync(admin)
                );
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Throw_Exception_If_UserName_Is_Null()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserType = (int)UserType.Admin
            };
            UserFriendlyException ex = await Assert
                .ThrowsAsync<UserFriendlyException>(
                    () => _iAdminAppSerice.CreateAdminAsync(admin)
                );
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Throw_Exception_If_Password_Is_Null()
        {
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                UserName = "123456",
                UserType = (int)UserType.Admin
            };
            UserFriendlyException ex = await Assert
                .ThrowsAsync<UserFriendlyException>(
                    () => _iAdminAppSerice.CreateAdminAsync(admin)
                );
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Throw_Exception_If_UserName_Length_Greater_Then_30()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 31; i++)
            {
                sb.Append(i);
            }
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                UserName = sb.ToString(),
                UserType = (int)UserType.Admin,
                Password = "123456"
            };
            UserFriendlyException ex = await Assert
                .ThrowsAsync<UserFriendlyException>(
                    () => _iAdminAppSerice.CreateAdminAsync(admin)
                );
            (ex != null).ShouldBe(true);
        }

        [Fact]
        public async void CreateAdminAsync_Throw_Exception_If_Password_Length_Greater_Then_23()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 24; i++)
            {
                sb.Append(i);
            }
            AdminDto admin = new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                UserName = "1234",
                UserType = (int)UserType.Admin,
                Password = sb.ToString()
            };
            UserFriendlyException ex = await Assert
                .ThrowsAsync<UserFriendlyException>(
                    () => _iAdminAppSerice.CreateAdminAsync(admin)
                    );
            (ex != null).ShouldBe(true);
        }
        #endregion

        #region DeleteAdmin
        [Fact]
        public void DeleteAdmin_Return_True_If_Success()
        {
            var admin = InitFakeEntity.GetFakeAdmin();
            Admin admin2 = UsingDbContext(ctx => ctx.Admin.Add(admin));
            _iAdminAppSerice.DeleteAdmin(admin2.UserId);
            var temp = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.UserId == admin2.UserId));
            temp.IsDelete.ShouldBe(true);
            Assert.True(temp.Id > 0);
        }

        [Fact]
        public void DeleteAdmin_Return_False_If_UserId_Not_Found()
        {
            var result = _iAdminAppSerice.DeleteAdmin(new Guid());
            Assert.False(result);
        }
        #endregion

        #region GetAdmin
        [Fact]
        public void GetAdmin_Return_Entity_If_Found()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            Admin admin2 = UsingDbContext(ctx => ctx.Admin.Add(admin));
            var result = _iAdminAppSerice.GetAdmin(admin2.UserId);
            Assert.True(result.Id > 0);
            result.UserId.ShouldBe(admin2.UserId);
        }

        [Fact]
        public void GetAdmin_Return_Empey_If_Not_Found()
        {
            var result = _iAdminAppSerice.GetAdmin(new Guid());
            Assert.True(result.Id == 0);
        }
        #endregion

        #region GetAllAdmin
        [Fact]
        public void GetAllAdmin__Should_Return_Correct_Number_Of_Records()
        {
            for (int i = 0; i < 8; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = admin.UserName + i.ToString();
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var count = UsingDbContext(ctx => ctx.Admin.Count());
            var result = _iAdminAppSerice.GetAllAdmin(1);
            result.ListData.Count.ShouldBe(count);
            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void GetAllAdmin_Return_Empty_Data()
        {
            UsingDbContext(ctx =>
            {
                var admin = ctx.Admin.First();
                ctx.Admin.Remove(admin);
            });
            UsingDbContext(ctx =>
            {
                var admin = ctx.Admin.First();
                ctx.Admin.Remove(admin);
            });
            var count = UsingDbContext(ctx => ctx.Admin.Count());
            var result = _iAdminAppSerice.GetAllAdmin(1);
            result.ListData.Count.ShouldBe(count);
            result.CurrentPage.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }
        #endregion

        #region GetAllInstitutionAdmin
        [Fact]
        public void GetAllInstitutionAdmin__Should_Return_Correct_Number_Of_Records()
        {
            var institution = new Institution
            {
                Address = "上海",
                District = "浦东新区",
                Name = "FirstInstitution2",
                Tel = "18817617807"
            };
            for (int i = 0; i < 10; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = admin.UserName + i.ToString();
                institution.Admins.Add(admin);
            }
            var institution2 = UsingDbContext(ctx => ctx.Institution.Add(institution));
            var result = _iAdminAppSerice.GetAllInstitutionAdmin(1, institution2.Id);
            result.ListData.Count.ShouldBe(10);
            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void GetAllInstitutionAdmin_Return_Empty()
        {
            var institution = new Institution
            {
                Address = "上海",
                District = "浦东新区",
                Name = "FirstInstitution2",
                Tel = "18817617807"
            };
            var institution2 = UsingDbContext(ctx => ctx.Institution.Add(institution));
            var result = _iAdminAppSerice.GetAllInstitutionAdmin(1, institution2.Id);
            result.ListData.Count.ShouldBe(0);
            result.CurrentPage.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }
        #endregion

        #region Login
        [Fact]
        public void Login_Return_Login_Success_If_Right()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            UsingDbContext(ctx => ctx.Admin.Add(admin));
            var result = _iAdminAppSerice.Login(admin.UserName, admin.Password);
            result.ErrorCount.ShouldBe(0);
            result.LockMintues.ShouldBe(0);
            result.LoginResult.ShouldBe(LoginResult.Success);
        }

        [Fact]
        public void Login_Return_PwdError_If_PwdError()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            UsingDbContext(ctx => ctx.Admin.Add(admin));
            var result = _iAdminAppSerice.Login(admin.UserName, "1");
            var admin2 = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.UserName == admin.UserName));
            result.ErrorCount.ShouldBe(0);
            result.LockMintues.ShouldBe(0);
            result.LoginResult.ShouldBe(LoginResult.PwdError);
            admin2.AccessFailedCount.ShouldBe(1);
        }

        [Fact]
        public void Login_Update_AccessFailedCount_2_If_PwdError_Count_2()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            UsingDbContext(ctx => ctx.Admin.Add(admin));
            _iAdminAppSerice.Login(admin.UserName, "1");
            var result = _iAdminAppSerice.Login(admin.UserName, "1");
            var admin2 = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.UserName == admin.UserName));
            result.LoginResult.ShouldBe(LoginResult.PwdError);
            admin2.AccessFailedCount.ShouldBe(2);
        }

        [Fact]
        public void Login_Return_AccountLock_If_PwdError_Count_3()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            UsingDbContext(ctx => ctx.Admin.Add(admin));
            _iAdminAppSerice.Login(admin.UserName, "1");
            _iAdminAppSerice.Login(admin.UserName, "1");
            var result = _iAdminAppSerice.Login(admin.UserName, "1");
            var admin2 = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.UserName == admin.UserName));
            result.LoginResult.ShouldBe(LoginResult.AccountLock);
            Assert.True(result.LockMintues > 0);
            admin2.AccessFailedCount.ShouldBe(3);
        }

        [Fact]
        public void Login_Return_Success_If_Account_Lock_And_LockTime_Expire()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            UsingDbContext(ctx => ctx.Admin.Add(admin));
            _iAdminAppSerice.Login(admin.UserName, "1");
            _iAdminAppSerice.Login(admin.UserName, "1");
            var result = _iAdminAppSerice.Login(admin.UserName, "1");
            var admin2 = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.UserName == admin.UserName));
            result.LoginResult.ShouldBe(LoginResult.AccountLock);
            Assert.True(result.LockMintues > 0);
            admin2.AccessFailedCount.ShouldBe(3);
            UsingDbContext(ctx =>
            {
                var a = ctx.Admin.FirstOrDefault(x => x.UserName == admin.UserName);
                if (a == null)
                    return;
                if (a.LastLoginDate.HasValue)
                {
                    a.LastLoginDate = a.LastLoginDate.Value.AddMinutes(-31);
                }
            });
            result = _iAdminAppSerice.Login(admin.UserName, admin.Password);
            result.ErrorCount.ShouldBe(0);
            result.LockMintues.ShouldBe(0);
            result.LoginResult.ShouldBe(LoginResult.Success);
        }
        #endregion

        #region UpdatePwd
        [Fact]
        public void UpdatePwd_Return_True_If_Success()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            string pwd = "1111";
            var admin2 = UsingDbContext(ctx => ctx.Admin.Add(admin));
            var result = _iAdminAppSerice.UpdatePwd(admin2.UserId, admin2.Password, pwd);
            var admin3 = UsingDbContext(ctx => ctx.Admin.First((x => x.UserId == admin2.UserId)));
            admin3.Password.ShouldBe(pwd);
            result.ShouldBe(true);
        }

        [Fact]
        public void UpdatePwd_Throw_If_OldPwd_Error()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            string pwd = "1112221";
            var admin2 = UsingDbContext(ctx => ctx.Admin.Add(admin));
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(() => _iAdminAppSerice.UpdatePwd(admin2.UserId, pwd, pwd));
            Assert.True(ex != null);
        }

        [Fact]
        public void UpdatePwd_Throw_If_User_Not_Found()
        {
            string pwd = "1111";
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(
                () => _iAdminAppSerice.UpdatePwd(new Guid(), "123456", pwd));
            Assert.True(ex != null);
        }
        #endregion

        #region CheckUserName
        [Fact]
        public void CheckUserName_Return_True_If_Not_Found()
        {
            var result = _iAdminAppSerice.CheckAdminUserName("aaaaaaa");
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckUserName_Return_False_If_Found()
        {
            Admin admin = InitFakeEntity.GetFakeAdmin();
            UsingDbContext(ctx => ctx.Admin.Add(admin));
            var result = _iAdminAppSerice.CheckAdminUserName(admin.UserName);
            result.ShouldBe(false);
        }
        #endregion

        #region SearchAdminPaination

        [Fact]
        public void SearchAdminPaination_Return_10_If_UserName_Exists()
        {
            var userName = "search";
            for (int i = 0; i < 10; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var list = UsingDbContext(ctx => ctx.Admin.Where(x => x.UserName.Contains(userName)).Count());
            list.ShouldBe(10);
            var result = _iAdminAppSerice.SearchAdminPaination(1, null, userName);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchAdminPaination_Return_Empty_If_UserName_Not_Found()
        {
            var userName = "search";
            var result = _iAdminAppSerice.SearchAdminPaination(1, null, userName);
            result.CurrentPage.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }

        [Fact]
        public void SearchAdminPaination_Should_Return_Correct_Number_Of_Records_If_UserName_Exists_And_UserType_Is_Admin()
        {
            var userName = "search";
            for (int i = 0; i < 20; i++)
            {
                UserType userType = i > 8 ? UserType.Admin : UserType.Teacher;
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                admin.UserType = userType;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, (int)UserType.Admin, userName);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(2);
        }

        [Fact]
        public void SearchAdminPaination_Should_Return_Correct_Number_Of_Records_If_UserName_Exists_And_UserType_Is_Teacher()
        {
            var userName = "search";
            for (int i = 0; i < 20; i++)
            {
                UserType userType = i > 9 ? UserType.Admin : UserType.Teacher;
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName+ i;
                admin.UserType = userType;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, (int)UserType.Teacher, userName);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchAdminPaination_Return_Empty_If_UserName_Not_Exists_And_UserType_Is_Teacher()
        {
            var userName = "search";
            for (int i = 0; i < 20; i++)
            {
                UserType userType = i > 9 ? UserType.Admin : UserType.Teacher;
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                admin.UserType = userType;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, (int)UserType.Teacher, "aaaa");
            result.CurrentPage.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }

        [Fact]
        public void SearchAdminPaination_Return_Empty_If_UserName_Not_Exists_And_UserType_Is_Admin()
        {
            var userName = "search";
            for (int i = 0; i < 20; i++)
            {
                UserType userType = i < 9 ? UserType.Admin : UserType.Teacher;
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                admin.UserType = userType;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, (int)UserType.Admin, "aaaaa");
            result.CurrentPage.ShouldBe(0);
            result.ListData.Count.ShouldBe(0);
            result.TotalPages.ShouldBe(0);
        }

        [Fact]
        public void SearchAdminPaination_Should_Return_Correct_Number_Of_Records_If_UserName_Null_And_UserType_Is_Null()
        {
            var userName = "search";
            for (int i = 0; i < 8; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                admin.UserType = UserType.Admin;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, null, "");
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchAdminPaination_Should_Return_Correct_Number_Of_Records_If_UserName_Null_And_UserType_Is_Admin()
        {
            var userName = "search";
            for (int i = 0; i < 8; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                admin.UserType = UserType.Admin;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, null, null);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void SearchAdminPaination_Should_Return_Correct_Number_Of_Records_If_UserName_Null_And_UserType_Is_Teacher()
        {
            var userName = "search";
            for (int i = 0; i < 8; i++)
            {
                Admin admin = InitFakeEntity.GetFakeAdmin();
                admin.UserName = userName + i;
                admin.UserType = UserType.Teacher;
                UsingDbContext(ctx => ctx.Admin.Add(admin));
            }
            var result = _iAdminAppSerice.SearchAdminPaination(1, null, null);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
            result.TotalPages.ShouldBe(1);
        }
        #endregion
    }
}
using Abp.Application.Services;
using Imagine.BookManager.Common;
using Imagine.BookManager.Dto.Admin;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Imagine.BookManager.AdminService
{
    public interface IAdminAppService : IApplicationService
    {
        [HttpGet]
        AdminDto GetAdmin(Guid userId);

        [HttpGet]
        PaginationDataList<AdminDto> GetAllAdmin(
            int? pageIndex,
            int? singletonPageCount=null);

        [HttpPost]
        int CreateAdmin(AdminDto admin);

        [HttpPost]
        Task<int> CreateAdminAsync(AdminDto admin);

        [HttpGet]
        PaginationDataList<AdminDto> GetAllInstitutionAdmin(
            int? pageIndex,
            int institutionId,
            int? singletonPageCount = null
            );

        [HttpGet]
        AdminLoginResult Login(string userName, string passWord);

        [HttpGet]
        bool DeleteAdmin(Guid userId);

        [HttpPost]
        bool UpdatePwd(Guid userId, string oldPwd, string newPwd);

        bool CheckAdminUserName(string userName);

        PaginationDataList<AdminDto> SearchAdminPaination(
            int? pageIndex,
            int? userType,
            string userName,
            int? singletonPageCount=null);

        PaginationDataList<AdminDto> SearchTeacherPaination(int? pageSize, int? pageRows, string teacherName, int? classId, int? setId, int setStatus, Guid userId);
    }
}

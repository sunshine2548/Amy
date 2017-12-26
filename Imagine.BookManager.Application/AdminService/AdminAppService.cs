using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Imagine.BookManager.AdminService
{
    public class AdminAppService : BookManagerAppServiceBase, IAdminAppService
    {
        private readonly IRepository<Admin> _adminRepository;

        private readonly int _passwordErrorCount;

        public AdminAppService(IRepository<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
            string strPasswordErrorCount = ConfigurationManager.AppSettings["PasswordErrorCount"];
            if (string.IsNullOrEmpty(strPasswordErrorCount))
            {
                _passwordErrorCount = 3;
                return;
            }
            if (int.TryParse(strPasswordErrorCount, out _passwordErrorCount) == false || _passwordErrorCount < 1)
            {
                _passwordErrorCount = 3;
            }
        }

        public IRepository<Institution> InstitutionRepository { get; set; }

        public int CreateAdmin(AdminDto admin)
        {
            var tempAdmin = _adminRepository.FirstOrDefault(x => x.UserName == admin.UserName);
            if (tempAdmin != null)
            {
                throw new UserFriendlyException("The user already exists");
            }
            if (admin.InstitutionId.HasValue)
            {
                var institution = InstitutionRepository.FirstOrDefault(admin.InstitutionId.Value);
                if (institution == null)
                {
                    throw new UserFriendlyException("The institution does not exist");
                }
            }
            var adminDto = ObjectMapper.Map<Admin>(admin);
            return _adminRepository.InsertAndGetId(adminDto);
        }

        public async Task<int> CreateAdminAsync(AdminDto admin)
        {
            var tempAdmin = _adminRepository.FirstOrDefault(x => x.UserName == admin.UserName);
            if (tempAdmin != null)
            {
                throw new UserFriendlyException("The user already exists");
            }
            if (admin.InstitutionId.HasValue)
            {
                var institution = InstitutionRepository.FirstOrDefault(admin.InstitutionId.Value);
                if (institution == null)
                {
                    throw new UserFriendlyException("The institution does not exist");
                }
            }
            var adminDto = ObjectMapper.Map<Admin>(admin);
            return await _adminRepository.InsertAndGetIdAsync(adminDto);

        }

        public bool DeleteAdmin(Guid userId)
        {
            Admin admin = _adminRepository.FirstOrDefault(x => x.UserId == userId);
            if (admin == null)
            {
                return false;
            }
            admin.IsDelete = true;
            return _adminRepository.Update(admin).IsDelete;
        }

        public AdminDto GetAdmin(Guid userId)
        {
            var tempAdmin = _adminRepository.FirstOrDefault(x => x.UserId == userId);
            AdminDto admin = new AdminDto();
            if (tempAdmin != null)
            {
                admin = ObjectMapper.Map<AdminDto>(tempAdmin);
                if (tempAdmin.InstitutionId.HasValue)
                {
                    var institution = InstitutionRepository.FirstOrDefault(tempAdmin.InstitutionId.Value);
                    admin.InstitutionName = institution == null ? "" : institution.Name;
                }
            }
            return admin;
        }

        public PaginationDataList<AdminDto> GetAllAdmin(
            int? pageIndex,
            int? singletonPageCount)
        {
            var list = _adminRepository.GetAll()
                .OrderByDescending(x => x.Id)
                .ToPagination(pageIndex, singletonPageCount);
            return ObjectMapper.Map<PaginationDataList<AdminDto>>(list);
        }

        public PaginationDataList<AdminDto> GetAllInstitutionAdmin(
            int? pageIndex,
            int institutionId,
            int? singletonPageCount = null)
        {
            PaginationDataList<Admin> list = _adminRepository.GetAll()
                .Where(x => x.InstitutionId == institutionId && x.InstitutionId.HasValue)
                .OrderByDescending(x => x.Id)
                .ToPagination(pageIndex, singletonPageCount);
            return ObjectMapper.Map<PaginationDataList<AdminDto>>(list);
        }

        public AdminLoginResult Login(string userName, string passWord)
        {
            AdminLoginResult result = new AdminLoginResult { LoginResult = LoginResult.PwdError };
            var admin = _adminRepository.FirstOrDefault(x => x.UserName == userName);
            if (admin == null)
            {
                return result;
            }
            if (admin.IsDelete)
            {
                return result;
            }
            if (admin.LastLoginDate.HasValue)
            {
                var minutes = (DateTime.Now - admin.LastLoginDate.Value).TotalMinutes;
                //短时间内密码错误次数太多，被锁定。
                if (minutes < 30 && admin.AccessFailedCount == _passwordErrorCount)
                {
                    result.LockMintues = (int)Math.Ceiling(minutes);
                    result.LoginResult = LoginResult.AccountLock;
                    result.ErrorCount = _passwordErrorCount;
                    return result;
                }
            }
            if (admin.Password == passWord)
            {
                admin.LastLoginDate = DateTime.Now;
                admin.AccessFailedCount = 0;
                _adminRepository.Update(admin);
                result.UerId = admin.UserId;
                result.LoginResult = LoginResult.Success;
                result.UserType = admin.UserType;
                return result;
            }
            if (admin.LastLoginDate.HasValue == false)
            {
                admin.LastLoginDate = DateTime.Now;
            }
            var lastLoginMinutes = (DateTime.Now - admin.LastLoginDate.Value).TotalMinutes;
            if (lastLoginMinutes > 30)
            {
                admin.AccessFailedCount = 1;
                result.LoginResult = LoginResult.PwdError;
            }
            else
            {
                if (admin.AccessFailedCount == _passwordErrorCount - 1)
                {
                    admin.AccessFailedCount = _passwordErrorCount;
                    result.LockMintues = 30;
                    result.LoginResult = LoginResult.AccountLock;
                }
                else
                {
                    admin.AccessFailedCount++;
                    result.LoginResult = LoginResult.PwdError;
                }
            }
            _adminRepository.Update(admin);
            return result;
        }

        public bool UpdatePwd(Guid userId, string oldPwd, string newPwd)
        {
            var admin = _adminRepository.FirstOrDefault(x => x.UserId == userId);
            if (admin == null)
            {
                throw new UserFriendlyException("User does not exist");
            }
            if (String.CompareOrdinal(admin.Password, oldPwd) != 0)
            {
                throw new UserFriendlyException("pwd error");
            }
            admin.Password = newPwd;
            return _adminRepository.Update(admin) != null;
        }

        public bool CheckAdminUserName(string userName)
        {
            var admin = _adminRepository.FirstOrDefault(x => x.UserName == userName);
            if (admin == null)
                return true;
            return false;
        }

        public PaginationDataList<AdminDto> SearchAdminPaination(
           int? pageIndex,
           int? userType,
           string userName,
           int? singletonPageCount = null)
        {
            userName = string.IsNullOrEmpty(userName) ? "" : userName;
            var type = userType ?? 0;
            if (type > 2 || type < 1)
                type = 0;
            var query = _adminRepository.GetAll()
                .Where(a => a.UserName.Contains(userName))
                .OrderByDescending(x => x.Id);
            if (type != 0)
            {
                query = _adminRepository
                    .GetAll()
                    .Where(a => a.UserName.Contains(userName) && a.UserType == (UserType)type)
                    .OrderByDescending(x => x.Id);
            }
            PaginationDataList<Admin> list = query.ToPagination(pageIndex, singletonPageCount);
            return ObjectMapper.Map<PaginationDataList<AdminDto>>(list);
        }

        public IRepository<ClassInfo> ClassInfoRepository { get; set; }
        public IRepository<Student, Int64> StudentRepository { get; set; }
        public IRepository<StudentAllocation, Int64> StudentAllocationRepository { get; set; }
        public IRepository<Set> SetRepository { get; set; }
        public IRepository<TeacherAllocation,Int64> TeacherAllocationRepository { get; set; }


        public PaginationDataList<AdminDto> SearchTeacherPaination(int? pageSize, int? pageRows, string teacherName,
            int? classId, int? setId, int setStatus, Guid userId)
        {
            teacherName = string.IsNullOrWhiteSpace(teacherName) ? string.Empty : teacherName;

            PaginationDataList<Admin> list = _adminRepository.GetAllIncluding(e => e.Classes)//.OrderByDescending(e => e.Id).ToPagination(pageSize, pageRows);
            .Where(e => e.UserName.Contains(teacherName) && e.UserType == UserType.Teacher && e.UserId != userId)
            .OrderByDescending(e => e.Id)
            .ToPagination(pageSize, pageRows);

            PaginationDataList<AdminDto> paginaList = new PaginationDataList<AdminDto>();
            paginaList.CurrentPage = pageSize.Value;
            var listData = list.ListData;
            
            for (int i = 0; i < listData.Count; i++)
            {
                var data = listData[i];
                var classes = listData[i].Classes;
                var dto = ObjectMapper.Map<AdminDto>(listData[i]);
                if (classId.HasValue && classId.Value != 0)
                {
                    classes = classes.Where(e => e.Id == classId).ToList();
                    if (classes.Count == 0)
                        continue;
                }

                var count = 0;
                var className = string.Empty;
                foreach (var item in classes)
                {
                    count = count + StudentRepository.Count(e => e.ClassId == item.Id);
                    if (string.IsNullOrEmpty(className))
                        className = item.Name;
                    else
                        className = className + "," + item.Name;
                }
                
                var teacherAllocations = TeacherAllocationRepository.GetAllList(e => e.TeacherId == data.UserId);
                if (setId.HasValue && setId.Value != 0)
                {

                    teacherAllocations = teacherAllocations.Where(e => e.SetId == setId.Value).ToList();
                    if (teacherAllocations.Count == 0)
                        continue;
                }

                if (setStatus == (int)SetAllotStatus.Allocated)
                {
                    if (teacherAllocations.Count == 0)
                        continue;
                }
                else if (setStatus == (int)SetAllotStatus.UnAllocated)
                {
                    if (teacherAllocations.Count > 0)
                        continue;

                }
                else if (setStatus == (int)SetAllotStatus.CreditInadequate)
                {
                    var b = false;
                    foreach (var item in teacherAllocations)
                    {
                        var sCount = StudentAllocationRepository.Count(e => e.TeacherAllocationId == item.Id);
                        if (item.Credit - sCount == 0)
                            b = true;
                        
                    }
                    if (!b)
                        continue;
                }

                var studentCount = 0;
                var setName = string.Empty;
                if (teacherAllocations.Count == 0)
                    setName = "未分配";
                else
                {
                    foreach (var item in teacherAllocations)
                    {
                        var setInfo = SetRepository.FirstOrDefault(e => e.Id == item.SetId);
                        var name = setInfo.SetName;
                        studentCount += StudentAllocationRepository.Count(e => e.TeacherAllocationId == item.Id);
                        if (string.IsNullOrEmpty(setName))
                            setName = name + "----" + studentCount.ToString() + "/" + item.Credit.ToString();
                        else
                            setName = setName + "," + name + "----" + studentCount.ToString() + "/" + item.Credit.ToString();
                    }
                }

                dto.StudentCount = count;
                dto.ClassName = className;
                dto.Password = null;
                dto.SetName = setName;
                paginaList.ListData.Add(dto);
            }
            paginaList.TotalPages = (int)Math.Ceiling(paginaList.ListData.Count * 1.0 / pageRows.Value);

            return paginaList;
        }
    }
}

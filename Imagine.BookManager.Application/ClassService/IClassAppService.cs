using Abp.Application.Services;
using Imagine.BookManager.AdminService;
using Imagine.BookManager.Common;
using System;
using System.Web.Http;
using Imagine.BookManager.Dto;
using Imagine.BookManager.Dto.Admin;
using Imagine.BookManager.Dto.Class;

namespace Imagine.BookManager.ClassService
{
    public interface IClassAppService : IApplicationService
    {
        [HttpPost]
        int CreateClassInfo(ClassInfoIn classInfo);
       
        [HttpGet]
        bool AllocationClassInfoToTeacher(int classId, Guid userId);

        [HttpPost]
        bool DeleteUsersClassInfo(int classId, Guid userId);
        [HttpGet]
        ClassInfoOut GetClassInfoById(int classId);
        [HttpGet]
        PaginationDataList<AdminDto> GetClassInfoTeachers(
            int classId,
            int? pageIndex,
            int? singletonPageCount = null);
        [HttpGet]
        bool AllocationStudentClass(Guid studentId, int classId);

        [HttpGet]
        PaginationDataList<ClassInfoOut> GetInstitutionAllClasses(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount = null);
    }
}

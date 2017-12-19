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
        
        int CreateClassInfo(ClassInfoIn classInfo);
       
        bool AllocationClassInfoToTeacher(int classId, Guid userId);

        bool DeleteUsersClassInfo(int classId, Guid userId);
        [HttpGet]
        ClassInfoOut GetClassInfoById(int classId);
        [HttpGet]
        PaginationDataList<AdminDto> GetClassInfoTeachers(
            int classId,
            int? pageIndex,
            int? singletonPageCount = null);
        bool AllocationStudentClass(Guid studentId, int classId);

        PaginationDataList<ClassInfoOut> GetInstitutionAllClasses(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount = null);
    }
}

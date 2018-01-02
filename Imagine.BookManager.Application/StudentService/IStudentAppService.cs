using Abp.Application.Services;
using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Imagine.BookManager.Common;
using Imagine.BookManager.Dto.Student;

namespace Imagine.BookManager.StudentService
{
    public interface IStudentAppService : IApplicationService
    {
        [HttpPost]
        Int64 CreateStudent(Student student);

        [HttpPost]
        void UpdateStudent(Student student);

        [HttpGet]
        PaginationDataList<StudentOut> GetAllStudentList(int? pageIndex, int? singletonPageCount = null);

        [HttpGet]
        bool CheckStudentName(string studentName);

        [HttpGet]
        PaginationDataList<StudentOut> SearchStudent(int? pageSize, int? pageRows, string name, int? classId,
            int setStatus, int? setId, string mobile, DateTime? startTime, Guid userId);
    }
}

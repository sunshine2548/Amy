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
        Int64 CreateStudent(Student student);

        void UpdateStudent(Student student);

        [HttpGet]
        PaginationDataList<StudentOut> GetAllStudentList(int? pageIndex, int? singletonPageCount = null);

        [HttpGet]
        bool CheckStudentName(string studentName);
    }
}

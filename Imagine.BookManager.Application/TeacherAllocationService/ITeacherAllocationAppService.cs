using Abp.Application.Services;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.TeacherAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Imagine.BookManager.TeacherAllocationService
{
    public interface ITeacherAllocationAppService : IApplicationService
    {
        [HttpPost]
        long CreatedTeacherAllocation(CreateTeacherAllocationInput input);

        [HttpPost]
        void DeletedTeacherAllocation(long teacherAllocationId);
        
        [HttpPost]
        bool UpdatedTeacherAllocation(UpdateTeacherAllocationInput input);
    }
}

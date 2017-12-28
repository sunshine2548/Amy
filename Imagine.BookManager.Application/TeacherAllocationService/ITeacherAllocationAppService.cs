using Abp.Application.Services;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.TeacherAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.TeacherAllocationService
{
    public interface ITeacherAllocationAppService : IApplicationService
    {
        long CreatedTeacherAllocation(CreateTeacherAllocationInput input);

        void DeletedTeacherAllocation(long teacherAllocationId);
        
        bool UpdatedTeacherAllocation(UpdateTeacherAllocationInput input);
    }
}

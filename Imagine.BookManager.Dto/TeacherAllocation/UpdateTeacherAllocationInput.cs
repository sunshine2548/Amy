using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Dto.TeacherAllocation
{
    [AutoMapTo(typeof(Core.Entity.TeacherAllocation))]
    public class UpdateTeacherAllocationInput : EntityDto<Int64>
    {
        public long OrderItemId { get; set; }
        public Guid TeacherId { get; set; }
        public int Credit { get; set; }
        public Core.Entity.Admin AdminObj { get; set; }
        public OrderItem OrderItemObj { get; set; }
        public Core.Entity.Set SetObj { get; set; }
    }
}

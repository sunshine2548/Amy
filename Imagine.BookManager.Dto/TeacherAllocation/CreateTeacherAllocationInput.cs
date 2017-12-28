using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.BookManager.Core.Entity;

namespace Imagine.BookManager.Dto.TeacherAllocation
{
    [AutoMapTo(typeof(Core.Entity.TeacherAllocation))]
    public class CreateTeacherAllocationInput : EntityDto
    {
        public long OrderItemId { get; set; }
        public Guid TeacherId { get; set; }
        public int Credit { get; set; }
        public Core.Entity.Admin AdminObj { get; set; }
        public OrderItem OrderItemObj { get; set; }
        public Core.Entity.Set SetObj { get; set; }
    }
}

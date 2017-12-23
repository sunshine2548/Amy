using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.TeacherAllocation;

namespace Imagine.BookManager.TeacherAllocationService
{
    public class TeacherAllocationAppService : BookManagerAppServiceBase, ITeacherAllocationAppService
    {
        public readonly IRepository<TeacherAllocation, Int64> _teacherAllocation;
        public readonly IRepository<OrderItem, Int64> _orderItem;
        public TeacherAllocationAppService(IRepository<TeacherAllocation, Int64> teacherAllocation, IRepository<OrderItem, Int64> orderItem)
        {
            _teacherAllocation = teacherAllocation;
            _orderItem = orderItem;
        }

        public long CreatedTeacherAllocation(CreateTeacherAllocationInput input)
        {
            if (input.TeacherId == Guid.Empty)
                throw new UserFriendlyException("The Teacher does not exists");
            if (input.Credit <= 0)
                throw new UserFriendlyException("Please Enter Credit");
            var orderItem = _orderItem.FirstOrDefault(e => e.Id == input.OrderItemId);
            if (orderItem == null)
                throw new UserFriendlyException("购买记录不存在");
            if ((orderItem.Quantity - orderItem.RemainCredit) < input.Credit)
                throw new UserFriendlyException("您输入的绘本额度过大，请重新输入");
            var teacherAllocation = ObjectMapper.Map<TeacherAllocation>(input);
            teacherAllocation.SetId = orderItem.SetId;
            var teacherAllocationId = _teacherAllocation.InsertAndGetId(teacherAllocation);
            if (teacherAllocationId > 0)
            {
                orderItem.RemainCredit = orderItem.RemainCredit + teacherAllocation.Credit;
                _orderItem.Update(orderItem);
            }
            return teacherAllocationId;
        }

        public void DeletedTeacherAllocation(long teacherAllocationId)
        {
            var teacherAllocation = _teacherAllocation.FirstOrDefault(e => e.Id == teacherAllocationId);
            if (teacherAllocation == null)
                throw new UserFriendlyException("该分配记录不存在");

            _teacherAllocation.Delete(teacherAllocation);
        }

        public bool UpdatedTeacherAllocation(UpdateTeacherAllocationInput input)
        {
            var teacherAllocation = _teacherAllocation.FirstOrDefault(e => e.Id == input.Id);
            if (teacherAllocation == null)
                throw new UserFriendlyException("该分配记录不存在");
            if (input.TeacherId == Guid.Empty)
                throw new UserFriendlyException("The Teacher does not exists");
            if (input.Credit <= 0)
                throw new UserFriendlyException("Please Enter Credit");
            var orderItem = _orderItem.FirstOrDefault(e => e.Id == input.OrderItemId);
            if (orderItem == null)
                throw new UserFriendlyException("购买记录不存在");
            if ((orderItem.Quantity - orderItem.RemainCredit) < input.Credit)
                throw new UserFriendlyException("您输入的绘本额度过大，请重新输入");
            teacherAllocation.SetId = orderItem.SetId;
            teacherAllocation.OrderItemId = orderItem.Id;
            teacherAllocation.TeacherId = input.TeacherId;
            teacherAllocation.Credit = input.Credit;
            teacherAllocation.DateAllocated = DateTime.Now;
            teacherAllocation.AdminObj = input.AdminObj;
            teacherAllocation.OrderItemObj = input.OrderItemObj;
            teacherAllocation.SetObj = input.SetObj;

            var result = _teacherAllocation.Update(teacherAllocation);
            orderItem.RemainCredit = orderItem.RemainCredit + teacherAllocation.Credit;
            _orderItem.Update(orderItem);
            return result.Id > 0;
        }
    }
}

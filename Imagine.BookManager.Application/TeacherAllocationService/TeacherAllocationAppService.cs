using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.TeacherAllocation;

namespace Imagine.BookManager.TeacherAllocationService
{
    public class TeacherAllocationAppService : BookManagerAppServiceBase, ITeacherAllocationAppService
    {
        public readonly IRepository<TeacherAllocation, Int64> _teacherAllocationRepo;
        public readonly IRepository<OrderItem, Int64> _orderItemRepo;
        public TeacherAllocationAppService(IRepository<TeacherAllocation, Int64> teacherAllocationRepo, IRepository<OrderItem, Int64> orderItemRepo)
        {
            _teacherAllocationRepo = teacherAllocationRepo;
            _orderItemRepo = orderItemRepo;
        }

        public long CreatedTeacherAllocation(CreateTeacherAllocationInput input)
        {
            if (input.TeacherId == Guid.Empty)
                throw new UserFriendlyException(ExceptionInfo.TeacherNotExists);
            if (input.Credit <= 0)
                throw new UserFriendlyException(ExceptionInfo.PleaseEnterCredit);
            var orderItem = _orderItemRepo.FirstOrDefault(e => e.Id == input.OrderItemId);
            if (orderItem == null)
                throw new UserFriendlyException(ExceptionInfo.PurchasedNoteNotExists);
            if ((orderItem.Quantity - orderItem.RemainCredit) < input.Credit)
                throw new UserFriendlyException(ExceptionInfo.CreditOverLimit);
            var teacherAllocation = ObjectMapper.Map<TeacherAllocation>(input);
            teacherAllocation.SetId = orderItem.SetId;
            var teacherAllocationId = _teacherAllocationRepo.InsertAndGetId(teacherAllocation);
            if (teacherAllocationId > 0)
            {
                orderItem.RemainCredit = orderItem.RemainCredit + teacherAllocation.Credit;
                _orderItemRepo.Update(orderItem);
            }
            return teacherAllocationId;
        }

        public void DeletedTeacherAllocation(long teacherAllocationId)
        {
            var teacherAllocation = _teacherAllocationRepo.FirstOrDefault(e => e.Id == teacherAllocationId);
            if (teacherAllocation == null)
                throw new UserFriendlyException(ExceptionInfo.CreditNoteNotExists);

            _teacherAllocationRepo.Delete(teacherAllocation);
        }

        public bool UpdatedTeacherAllocation(UpdateTeacherAllocationInput input)
        {
            var teacherAllocation = _teacherAllocationRepo.FirstOrDefault(e => e.Id == input.Id);
            if (teacherAllocation == null)
                throw new UserFriendlyException(ExceptionInfo.CreditNoteNotExists);
            if (input.TeacherId == Guid.Empty)
                throw new UserFriendlyException(ExceptionInfo.TeacherNotExists);
            if (input.Credit <= 0)
                throw new UserFriendlyException(ExceptionInfo.PleaseEnterCredit);
            var orderItem = _orderItemRepo.FirstOrDefault(e => e.Id == input.OrderItemId);
            if (orderItem == null)
                throw new UserFriendlyException(ExceptionInfo.PurchasedNoteNotExists);
            if ((orderItem.Quantity - orderItem.RemainCredit) < input.Credit)
                throw new UserFriendlyException(ExceptionInfo.CreditOverLimit);
            teacherAllocation.SetId = orderItem.SetId;
            teacherAllocation.OrderItemId = orderItem.Id;
            teacherAllocation.TeacherId = input.TeacherId;
            teacherAllocation.Credit = input.Credit;
            teacherAllocation.DateAllocated = DateTime.Now;

            var result = _teacherAllocationRepo.Update(teacherAllocation);
            orderItem.RemainCredit = orderItem.RemainCredit + teacherAllocation.Credit;
            _orderItemRepo.Update(orderItem);
            return result.Id > 0;
        }
    }
}

using Abp.UI;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.TeacherAllocation;
using Imagine.BookManager.EntityFramework;
using Imagine.BookManager.TeacherAllocationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class TeacherAllocationAppServiceTest : BookManagerTestBase
    {
        public readonly ITeacherAllocationAppService _teacherAllocationAppService;
        public TeacherAllocationAppServiceTest() => _teacherAllocationAppService = Resolve<ITeacherAllocationAppService>();

        #region Create
        [Fact]
        public void CreateTeacherAllocation_Return_True_If_Success()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));
            var teacherAllocationDto = new CreateTeacherAllocationInput()
            {
                OrderItemId = orderItem.Id,
                TeacherId = admin.UserId,
                Credit = 100,
                OrderItemObj = orderItem,
                SetObj = set,
                AdminObj = admin
            };

            long teacherAllocationId = _teacherAllocationAppService.CreatedTeacherAllocation(teacherAllocationDto);
            Assert.True(teacherAllocationId > 0);
        }

        [Fact]
        public void CreateTeacherAllocation_Return_True_If_Success_TeacherId_NoEmpty()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));
            var teacherAllocationDto = new CreateTeacherAllocationInput()
            {
                OrderItemId = orderItem.Id,
                TeacherId = Guid.Empty,
                Credit = 100,
                OrderItemObj = orderItem,
                SetObj = set,
                AdminObj = admin
            };

            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.CreatedTeacherAllocation(teacherAllocationDto);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void CreateTeacherAllocation_Return_True_If_Success_Credit_Zero()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));
            var teacherAllocationDto = new CreateTeacherAllocationInput()
            {
                OrderItemId = orderItem.Id,
                TeacherId = admin.UserId,
                Credit = 0,
                OrderItemObj = orderItem,
                SetObj = set,
                AdminObj = admin
            };

            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.CreatedTeacherAllocation(teacherAllocationDto);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void CreateTeacherAllocation_Return_True_If_Success_Credit_Exceed()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));
            var teacherAllocationDto = new CreateTeacherAllocationInput()
            {
                OrderItemId = orderItem.Id,
                TeacherId = admin.UserId,
                Credit = 1000,
                OrderItemObj = orderItem,
                SetObj = set,
                AdminObj = admin
            };

            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.CreatedTeacherAllocation(teacherAllocationDto);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void CreateTeacherAllocation_Return_True_If_Success_OrderItem_Null()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));
            var teacherAllocationDto = new CreateTeacherAllocationInput()
            {
                OrderItemId = 0,
                TeacherId = admin.UserId,
                Credit = 100,
                OrderItemObj = orderItem,
                SetObj = set,
                AdminObj = admin
            };

            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.CreatedTeacherAllocation(teacherAllocationDto);
            });
            Assert.True(ex != null);
        }
        #endregion

        #region Update
        [Fact]
        public void UpdateTeacherAllocation_Return_True_If_Success()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = orderItem.Id;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            var input = new UpdateTeacherAllocationInput()
            {
                Id = teacherAllocation.Id,
                Credit = teacherAllocation.Credit + 100,
                OrderItemId = teacherAllocation.OrderItemId,
                TeacherId = teacherAllocation.TeacherId
            };

            var result = _teacherAllocationAppService.UpdatedTeacherAllocation(input);
            Assert.True(result);
        }

        [Fact]
        public void UpdateTeacherAllocation_Return_True_If_Success_TeacherId_Empty()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = orderItem.Id;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            var input = new UpdateTeacherAllocationInput()
            {
                Id = teacherAllocation.Id,
                Credit = teacherAllocation.Credit + 100,
                OrderItemId = teacherAllocation.OrderItemId,
                TeacherId = Guid.Empty
            };
            
            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.UpdatedTeacherAllocation(input);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void UpdateTeacherAllocation_Return_True_If_Success_TeacherAllocation_Invalid()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = orderItem.Id;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            var input = new UpdateTeacherAllocationInput()
            {
                Id = 0,
                Credit = teacherAllocation.Credit,
                OrderItemId = teacherAllocation.OrderItemId,
                TeacherId = teacherAllocation.TeacherId
            };

            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.UpdatedTeacherAllocation(input);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void UpdateTeacherAllocation_Return_True_If_Success_Credit_Zero()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = orderItem.Id;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            var input = new UpdateTeacherAllocationInput()
            {
                Id = teacherAllocation.Id,
                Credit = 0,
                OrderItemId = teacherAllocation.OrderItemId,
                TeacherId = teacherAllocation.TeacherId
            };
            
            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.UpdatedTeacherAllocation(input);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void UpdateTeacherAllocation_Return_True_If_Success_OrderItem_Null()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = orderItem.Id;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            var input = new UpdateTeacherAllocationInput()
            {
                Id = teacherAllocation.Id,
                Credit = teacherAllocationEntity.Credit,
                OrderItemId = 0,
                TeacherId = teacherAllocation.TeacherId
            };
            
            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.UpdatedTeacherAllocation(input);
            });
            Assert.True(ex != null);
        }

        [Fact]
        public void UpdateTeacherAllocation_Return_True_If_Success_Credit_Exceed()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = 0;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            var input = new UpdateTeacherAllocationInput()
            {
                Id = teacherAllocation.Id,
                Credit = 1000,
                OrderItemId = teacherAllocation.OrderItemId,
                TeacherId = teacherAllocation.TeacherId
            };
            
            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.UpdatedTeacherAllocation(input);
            });
            Assert.True(ex != null);
        }
        #endregion

        #region Delete

        [Fact]
        public void DeleteTeacherAllocation_Return_True_If_Success_TeacherAllocationId_Zero()
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(e => e.Admin.Add(adminEntity));
            var orderEntity = InitFakeEntity.GetFakeOrder();
            orderEntity.UserId = admin.UserId;
            orderEntity.Paid = true;
            var order = UsingDbContext(e => e.Order.Add(orderEntity));
            var setEntity = InitFakeEntity.GetFakeSet();
            var set = UsingDbContext(e => e.Sets.Add(setEntity));
            var orderItemEntity = InitFakeEntity.GetFakeOrderItem();
            orderItemEntity.UserId = admin.UserId;
            orderItemEntity.SetId = set.Id;
            orderItemEntity.OrderRef = order.OrderRef;
            var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemEntity));

            var teacherAllocationEntity = InitFakeEntity.GetFakeTeacherAllocation();
            teacherAllocationEntity.OrderItemId = orderItem.Id;
            teacherAllocationEntity.SetId = set.Id;
            teacherAllocationEntity.TeacherId = admin.UserId;
            teacherAllocationEntity.OrderItemObj = orderItem;
            teacherAllocationEntity.SetObj = set;
            teacherAllocationEntity.AdminObj = admin;
            var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationEntity));

            UserFriendlyException ex = Assert.Throws<UserFriendlyException>(() =>
            {
                _teacherAllocationAppService.DeletedTeacherAllocation(0);
            });
            Assert.True(ex != null);
        }
        #endregion
    }
}

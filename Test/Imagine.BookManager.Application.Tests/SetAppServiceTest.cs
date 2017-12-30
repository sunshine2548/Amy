using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Set;
using Imagine.BookManager.SetService;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class SetAppServiceTest : BookManagerTestBase
    {
        private readonly ISetAppService _setAppService;

        public SetAppServiceTest()
        {
            _setAppService = Resolve<ISetAppService>();
        }

        [Fact]
        public void CreateSet_Return_True_If_Success()
        {
            var createSet = InitFakeEntity.GetFakeSetDto();
            _setAppService.CreateSet(createSet);
            var set = UsingDbContext(ctx => ctx.Sets.First());
            createSet.SetName.ShouldBe(set.SetName);
        }

        [Fact]
        public void CreateSet_Throw_Exception_If_Name_Exists()
        {
            var set = InitFakeEntity.GetFakeSet();
            var createSet = InitFakeEntity.GetFakeSetDto();
            createSet.SetName = set.SetName;
            UsingDbContext(ctx => ctx.Sets.Add(set));
            UserFriendlyException ex = Should.Throw<UserFriendlyException>(
                () => _setAppService.CreateSet(createSet));
            Assert.True(ex != null);
        }

        [Fact]
        public void CheckSetName_Return_Ture_If_Not_Found()
        {
            var set = InitFakeEntity.GetFakeSet();
            UsingDbContext(ctx => ctx.Sets.Add(set));
            var result = _setAppService.CheckSetName(set.SetName + "aaa");
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckSetName_Return_False_If_Found()
        {
            var set = InitFakeEntity.GetFakeSet();
            UsingDbContext(ctx => ctx.Sets.Add(set));
            var result = _setAppService.CheckSetName(set.SetName);
            result.ShouldBe(false);
        }

        [Fact]
        public void GetSetById_Return_Empty()
        {
            var result = _setAppService.GetSetById(100);
            result.Id.ShouldBe(0);
        }

        [Fact]
        public void GetSetById_Return_Set_If_Found()
        {
            var set = UsingDbContext(ctx => ctx.Sets.Add(InitFakeEntity.GetFakeSet()));
            var result = _setAppService.GetSetById(set.Id);
            result.Id.ShouldBe(set.Id);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public void GetAllSet_Should_Return_Correct_Number_Of_Records()
        {
            //Arrange:insert 10 sets
            for (int i = 0; i < 10; i++)
            {
                var set = InitFakeEntity.GetFakeSet();
                set.SetName = set.SetName + i;
                UsingDbContext(ctx => ctx.Sets.Add(set));
            }
            var list = _setAppService.GetAllSet();
            list.Count.ShouldBe(10);
        }

        [Fact]
        public void GetAllSet_Return_Empty()
        {
            var list = _setAppService.GetAllSet();
            list.Count.ShouldBe(0);
        }

        #region SearchPicBook
        private Guid SetData(int type)
        {
            var institution = UsingDbContext(e => e.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminDto = InitFakeEntity.GetFakeAdmin();
            adminDto.UserId = Guid.NewGuid();
            adminDto.UserType = UserType.Admin;
            adminDto.InstitutionId = institution.Id;

            var classDto = InitFakeEntity.GetFakeClassInfo();
            classDto.Admins.Add(adminDto);
            classDto.InstitutionId = institution.Id;

            for (int i = 0; i < 3; i++)
            {
                var studentDto = InitFakeEntity.GetFakeStudent();
                studentDto.StudentId = Guid.NewGuid();
                studentDto.UserName = studentDto.UserName + i;
                classDto.Students.Add(studentDto);
            }
            var classInfo = UsingDbContext(e => e.ClassInfo.Add(classDto));
            var userId = Guid.Empty;
            for (int i = 0; i < 3; i++)
            {
                var setDto = InitFakeEntity.GetFakeSet();
                setDto.SetName = setDto.SetName + i;
                for (int j = 0; j < 3; j++)
                {
                    var bookDto = InitFakeEntity.GetFakeBook();
                    bookDto.BookName = bookDto.BookName + i + j;
                    setDto.Books.Add(bookDto);
                }

                var set = UsingDbContext(e => e.Sets.Add(setDto));

                var orderDto = InitFakeEntity.GetFakeOrder();
                orderDto.UserId = classInfo.Admins.First().UserId;
                orderDto.OrderRef = orderDto.OrderRef + i;
                orderDto.Paid = true;
                var order = UsingDbContext(e => e.Order.Add(orderDto));

                var adminDto2 = InitFakeEntity.GetFakeAdmin();
                adminDto2.UserName = adminDto2.UserName + i;
                adminDto2.UserId = Guid.NewGuid();
                adminDto2.InstitutionId = institution.Id;

                var classDto2 = InitFakeEntity.GetFakeClassInfo();
                classDto2.Admins.Add(adminDto2);
                classDto2.InstitutionId = institution.Id;
                classDto2.Name = classDto2.Name + i;

                for (int k = 0; k < 3; k++)
                {
                    var studentDto = InitFakeEntity.GetFakeStudent();
                    studentDto.StudentId = Guid.NewGuid();
                    studentDto.UserName = studentDto.UserName + i + k;
                    classDto2.Students.Add(studentDto);
                }
                var classInfo2 = UsingDbContext(e => e.ClassInfo.Add(classDto2));
                userId = classInfo2.Admins.First().UserId;


                var payment = InitFakeEntity.GetFakePayment();
                payment.GatewayRef = payment.GatewayRef + i;
                payment.Paid = true;
                payment.OrderRef = order.OrderRef;
                UsingDbContext(e => e.Payment.Add(payment));



                var orderItemDto = InitFakeEntity.GetFakeOrderItem();
                orderItemDto.SetId = set.Id;
                orderItemDto.Set = set;
                orderItemDto.OrderRef = order.OrderRef;
                orderItemDto.UserId = order.UserId;
                var orderItem = UsingDbContext(e => e.OrderItem.Add(orderItemDto));

                

                var teacherAllocationDto = InitFakeEntity.GetFakeTeacherAllocation();
                teacherAllocationDto.OrderItemId = orderItem.Id;
                teacherAllocationDto.SetId = set.Id;
                teacherAllocationDto.TeacherId = classInfo2.Admins.First().UserId;
                foreach (var item in classInfo2.Students)
                {
                    var studentAllocationDto = new StudentAllocation
                    {
                        StudentId = item.StudentId
                    };
                    teacherAllocationDto.StudentAllocations.Add(studentAllocationDto);
                }
                
                var teacherAllocation = UsingDbContext(e => e.TeacherAllocation.Add(teacherAllocationDto));

            }
            if (type == 1)
                return classInfo.Admins.First().UserId;
            else
                return userId;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void SearchPicBook_Return_If_Teacher_True(int setStatus)
        {
            var userId = SetData(2);
            var list = SearchPicBook(setStatus, userId);
            Assert.True(list.ListData.Count > 0);
            Assert.True(list.CurrentPage == 1);
            Assert.True(list.TotalPages == 1);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void SearchPicBook_Return_If_Teacher_False(int setStatus)
        {
            var userId = SetData(2);
            var list = SearchPicBook(setStatus, userId);
            Assert.False(list.ListData.Count != 0);
            Assert.False(list.CurrentPage != 0);
            Assert.False(list.TotalPages != 0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void SearchPicBook_Return_If_Admin_True(int setStatus)
        {
            var userId = SetData(1);
            var list = SearchPicBook(setStatus, userId);
            Assert.True(list.ListData.Count > 0);
            Assert.True(list.CurrentPage == 1);
            Assert.True(list.TotalPages == 1);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void SearchPicBook_Return_If_Admin_False(int setStatus)
        {
            var userId = SetData(1);
            var list = SearchPicBook(setStatus, userId);
            Assert.False(list.ListData.Count != 0);
            Assert.False(list.CurrentPage != 0);
            Assert.False(list.TotalPages != 0);
        }

        private PaginationDataList<SetDto> SearchPicBook(int setStatus,Guid userId) 
        {
            return _setAppService.SearchPicBook(1, 10, setStatus, userId);
        }
        #endregion
    }
}
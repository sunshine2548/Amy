using Abp.UI;
using Imagine.BookManager.SetService;
using Shouldly;
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
    }
}
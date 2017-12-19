using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;
using System.Configuration;

namespace Imagine.BookManager.Common.Tests
{
    public class PaginnationTest
    {
        [Fact]
        public void GetPageIndex_Return_1_If_Parameter_Is_Null()
        {
            var result = PaginationCommon.GetPageIndex(null);
            result.ShouldBe(1);
        }

        [Fact]
        public void GetPageIndex_Return_value_If_Parameter_Is_Not_Null()
        {
            var result = PaginationCommon.GetPageIndex(10);
            result.ShouldBe(10);
        }

        [Fact]
        public void GetPageIndex_Return_1_If_Parameter_Less_Than_0()
        {
            var result = PaginationCommon.GetPageIndex(-1);
            result.ShouldBe(1);
        }

        [Fact]
        public void GetSingletonPageCount_Return_Default_Value_If_Parameter_Is_Null()
        {
            var vaule = ConfigurationManager.AppSettings["SingletonPageCount"];
            var result = PaginationCommon.GetSingletonPageCount(null);
            result.ToString().ShouldBe(vaule);
        }

        [Fact]
        public void GetSingletonPageCount_Return_Value_If_Parameter_Not_Null()
        {
            var result = PaginationCommon.GetSingletonPageCount(10);
            result.ShouldBe(10);
        }

        [Fact]
        public void GetSingletonPageCount_Return_Default_If_Parameter_Lesson_0()
        {
            var result = PaginationCommon.GetSingletonPageCount(-1);
            result.ShouldBe(10);
        }

        [Fact]
        public void ToPagination_Get_First_Page()
        {
            List<PersonTest> list = new List<PersonTest>();
            for (var i = 0; i < 30; i++)
            {
                list.Add(new PersonTest() { Age = i });
            }
            int pageCount = (int)(Math.Ceiling(list.Count * 1.0 / 10));
            var result = list.AsQueryable().ToPagination(1, 10);
            result.TotalPages.ShouldBe(pageCount);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void ToPagination_Return_Default_Page_If_Singleton_Is_Null()
        {
            List<PersonTest> list = new List<PersonTest>();
            for (var i = 0; i < 30; i++)
            {
                list.Add(new PersonTest() { Age = i });
            }
            int pageCount = (int)(Math.Ceiling(list.Count * 1.0 / 10));
            var result = list.AsQueryable().ToPagination(1, null);
            result.TotalPages.ShouldBe(pageCount);
            result.CurrentPage.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        class PersonTest
        {
            public int Age { get; set; }
        }
    }
}
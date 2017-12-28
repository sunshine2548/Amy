using System;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Admin;
using Imagine.BookManager.Dto.Set;

namespace Imagine.BookManager.Application.Tests
{
    public class InitFakeEntity
    {
        public static ShoppingCart GetFakeShoppingCart()
        {
            return new ShoppingCart()
            {
                TotalQuantity = 10,
                Discount = 1000,
                Total = 2000,
                Timestamp = DateTime.Now
            };
        }

        public static Institution GetFakeInstitution()
        {
            return new Institution
            {
                Address = "上海",
                District = "上海市浦东新区",
                Name = "想象力英语",
                Tel = "123456"
            };
        }

        public static AdminDto GetFakeAdminDto()
        {
            return new AdminDto
            {
                Email = "brian@imaginelearning.cn",
                FullName = "brian2",
                Gender = true,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "brian2",
                UserType = (int)UserType.Admin
            };
        }

        public static Admin GetFakeAdmin()
        {
            return new Admin
            {
                DateCreated = DateTime.Now,
                Email = "brian2@imaginelearning.cn",
                FullName = "brian",
                Gender = true,
                IsDelete = false,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "testClass",
                UserType = UserType.Teacher
            };
        }

        public static Student GetFakeStudent()
        {
            return new Student
            {
                UserName = "brian",
                DateCreated = DateTime.Now,
                FullName = "12345",
                Gender = true,
                GuardianName = "Brian",
                IsDelete = false,
                Mobile = "1234567",
                Password = "123456",
                Picture = "111111111"
            };
        }

        public static Set GetFakeSet()
        {
            return new Set()
            {
                Synopsis = "123",
                ImageUrl = "132123/png",
                Price = 200,
                SetName = "Abc",
                OriginalPrice = 210
            };
        }

        public static SetDto GetFakeSetDto()
        {
            return new SetDto()
            {
                Synopsis = "123",
                ImageUrl = "132123/png",
                Price = 200,
                OriginalPrice = 210,
                SetName = "Abc"
            };
        }

        public static CartItem GetFakeCartItem(int setId = 0, int quantity = 1, decimal price = 100, decimal discount = 90)
        {
            return new CartItem()
            {
                Price = price,
                Quantity = quantity,
                Timestamp = DateTime.Now,
                Discount = discount,
                SetId = setId
            };
        }

        public static Order GetFakeOrder()
        {
            return new Order
            {
                DeliveryCharge = 1000,
                Discount = 1000,
                OrderRef = "1234567",
                Subtotal = 1000,
                Timestamp = DateTime.Now,
                Total = 10000,
                TotalQuantity = 100
            };
        }

        public static OrderItem GetFakeOrderItem()
        {
            return new OrderItem()
            {
                Price = 1000,
                Discount = 9,
                Quantity = 1000,
                RemainCredit = 200
            };
        }

        public static TeacherAllocation GetFakeTeacherAllocation()
        {
            return new TeacherAllocation()
            {
                Credit = 100
            };
        }
    }
}
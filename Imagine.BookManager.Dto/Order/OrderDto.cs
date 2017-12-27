using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;

namespace Imagine.BookManager.Dto.Order
{
    [AutoMapTo(typeof(Core.Entity.Order)), AutoMapFrom(typeof(Core.Entity.Order))]
    public class OrderDto : EntityDto
    {
        public string OrderRef { get; set; }
        public Guid UserId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public DateTime Timestamp { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public OrderDto()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}

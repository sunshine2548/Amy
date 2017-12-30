using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Imagine.BookManager.Core.Entity;

namespace Imagine.BookManager.Dto.Set
{
    [AutoMapFrom(typeof(Core.Entity.Set)), AutoMapTo(typeof(Core.Entity.Set))]
    public class SetDto : EntityDto
    {
        public string SetName { get; set; }
        public string Synopsis { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public ICollection<Book> Books { get; set; }
        public DateTime? DateCreated { get; set; }
        public string SurplusCredit { get; set; }
        public int ExpiryTime { get; set; }
        public string SetType { get; set; }

        public SetDto()
        {
            Books = new List<Book>();
            ExpiryTime = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("UsefulTime"));
        }
    }
}
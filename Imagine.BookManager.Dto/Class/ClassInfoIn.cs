using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Imagine.BookManager.Core.Entity;

namespace Imagine.BookManager.Dto.Class
{
    [AutoMapTo(typeof(ClassInfo))]
    public class ClassInfoIn : EntityDto
    {
        public string Name { get; set; }
        public int? ReminderInterva { get; set; }
        public Guid UserId { get; set; }
    }
}

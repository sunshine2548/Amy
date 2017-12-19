using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Imagine.BookManager.Core.Entity;

namespace Imagine.BookManager.Dto.Class
{
    [AutoMapFrom(typeof(ClassInfo))]
    public class ClassInfoOut:EntityDto
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ReminderInterva { get; set; }
        public int InstitutionId { get; set; }
    }
}

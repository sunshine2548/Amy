using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace Imagine.BookManager.Dto.Student
{
    [AutoMapFrom(typeof(Core.Entity.Student))]
    public class StudentOut : EntityDto<Int64>
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Picture { get; set; }
        public string GuardianName { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ClassId { get; set; }
        public string ClassName { get; set; }
        public int IsDelete { get; set; }
    }
}

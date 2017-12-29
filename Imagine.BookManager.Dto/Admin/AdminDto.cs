using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using Abp.UI;
using System.Collections.Generic;

namespace Imagine.BookManager.Dto.Admin
{
    [AutoMapFrom(typeof(Core.Entity.Admin)), AutoMapTo(typeof(Core.Entity.Admin))]
    public class AdminDto : EntityDto, ICustomValidate
    {
        public Guid? UserId { get; set; }
        public string InstitutionName { get; set; }
        public int? InstitutionId { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public int UserType { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Password { get; set; }
        public int StudentCount { get; set; }
        public IList<string> ClassName { get; } = new List<string>();
        public IList<string> SetName { get; } = new List<string>();

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                context.Results.Add(new ValidationResult("UserName is null"));
                throw new UserFriendlyException("Username is null");
            }
            if (string.IsNullOrEmpty(Password))
            {
                context.Results.Add(new ValidationResult("Password is null"));
                throw new UserFriendlyException("Password is null");
            }
            if (UserName.Length > 30)
            {
                context.Results.Add(new ValidationResult("UserName is not valid"));
                throw new UserFriendlyException("UserName is not valid");
            }
            if (Password.Length > 23)
            {
                context.Results.Add(new ValidationResult("Password is not valid"));
                throw new UserFriendlyException("Password is not valid");
            }
        }
    }
}

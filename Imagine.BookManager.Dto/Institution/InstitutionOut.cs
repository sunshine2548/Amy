using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Imagine.BookManager.Dto.Institution
{
    [AutoMapFrom(typeof(Core.Entity.Institution)), AutoMapTo(typeof(Core.Entity.Institution))]
    public class InstitutionOut:EntityDto
    {
        public string Name { get; set; }
        public string Tel { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }
}

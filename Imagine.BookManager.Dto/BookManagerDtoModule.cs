using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Admin;
using Imagine.BookManager.Dto.Class;
using Imagine.BookManager.Dto.Institution;
using Imagine.BookManager.Dto.Order;
using Imagine.BookManager.Dto.Set;
using Imagine.BookManager.Dto.Student;

namespace Imagine.BookManager.Dto
{
    [DependsOn(typeof(BookManagerCoreModule), typeof(AbpAutoMapperModule))]
    public class BookManagerDtoModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.CreateMap<AdminDto, Core.Entity.Admin>()
                    .ForMember(x => x.AccessFailedCount, opt => opt.Ignore())
                    .ForMember(x => x.DateCreated, opt => opt.Ignore())
                    .ForMember(x => x.Id, opt => opt.Ignore())
                    .ForMember(x => x.UserId, opt => opt.Ignore())
                    .ForMember(x => x.IsDelete, opt => opt.Ignore())
                    .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
                    .ForMember(x => x.Orders, opt => opt.Ignore())
                    .ForMember(x => x.ShoppingCarts, opt => opt.Ignore())
                    .ForMember(x => x.Classes, opt => opt.Ignore());

                cfg.CreateMap<Core.Entity.Admin, AdminDto>();

                cfg.CreateMap<ClassInfoIn, ClassInfo>()
                    .ForMember(x => x.Id, opt => opt.Ignore())
                    .ForMember(x => x.DateCreated, opt => opt.Ignore())
                    .ForMember(x => x.Admins, opt => opt.Ignore())
                    .ForMember(x => x.InstitutionId, opt => opt.Ignore());



                cfg.CreateMap<Core.Entity.Student, StudentOut>()
                    .ForMember(x => x.IsDelete, opt => opt.MapFrom(s => s.IsDelete ? 1 : 0));



                cfg.CreateMap<OrderDto, Core.Entity.Order>()
                    .ForMember(o => o.ShoppingCarts, opt => opt.Ignore())
                    .ForMember(o => o.Payments, opt => opt.Ignore());

                cfg.CreateMap<CartItem, OrderItem>();
                cfg.CreateMap<ShoppingCart, Core.Entity.Order>();
            });
        }
    }
}
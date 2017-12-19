using Abp.Application.Services;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Order;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Imagine.BookManager.OrderService
{
    public interface IOrderAppService : IApplicationService
    {
        [HttpPost]
        bool CreateOrder(OrderDto order);

        [HttpGet]
        PaginationDataList<OrderDto> GetAll(int? pageIndex, int? singletonPageCount);

        [HttpGet]
        OrderDto GetOrderByOrderRef(string orderRef);
        
        [HttpGet]
        bool UpdateOrderPaid(string orderRef, bool paid);

        [HttpGet]
        PaginationDataList<OrderDto> GetUserOrders(
            Guid userId,
            int? pageIndex,
            int? singletonPageCount);

        [HttpGet]
        PaginationDataList<OrderDto> GetInstitutionOrderList(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount
            );


    }
}

using Abp.Domain.Repositories;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Order;
using System;
using System.Linq;

namespace Imagine.BookManager.OrderService
{
    public class OrderAppService : BookManagerAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, Int64> _orderRepository;

        public IRepository<Institution> InstitutionRepository { get; set; }

        public OrderAppService(IRepository<Order, Int64> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool CreateOrder(OrderDto order)
        {
            var orderTemp = _orderRepository.FirstOrDefault(o => o.OrderRef == order.OrderRef);
            if (orderTemp != null)
            {
                throw new UserFriendlyException(ExceptionInfo.OrderRefExists);
            }
            var orderData = ObjectMapper.Map<Order>(order);
            return _orderRepository.InsertAndGetId(orderData) > 0;
        }

        public PaginationDataList<OrderDto> GetAll(int? pageIndex, int? singletonPageCount)
        {
            var list = _orderRepository.GetAllIncluding(x => x.OrderItems).OrderByDescending(x => x.Id);
            return ObjectMapper.Map<PaginationDataList<OrderDto>>(
                list.ToPagination(pageIndex, singletonPageCount)
                );
        }

        public OrderDto GetOrderByOrderRef(string orderRef)
        {
            var result = _orderRepository.GetAllIncluding(x => x.OrderItems).FirstOrDefault(x => orderRef == x.OrderRef);
            return ObjectMapper.Map<OrderDto>(result);
        }



        public bool UpdateOrderPaid(string orderRef, bool paid)
        {
            var order = _orderRepository.FirstOrDefault(x => orderRef == x.OrderRef);
            if (order == null)
            {
                return false;
            }
            order.Paid = paid;
            return _orderRepository.Update(order) != null;
        }

        public PaginationDataList<OrderDto> GetUserOrders(
            Guid userId,
            int? pageIndex,
            int? singletonPageCount)
        {
            var queryable = _orderRepository
                .GetAllIncluding(x => x.OrderItems)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id);
            return ObjectMapper
                .Map<PaginationDataList<OrderDto>>(
                queryable.ToPagination(pageIndex, singletonPageCount));
        }

        public PaginationDataList<OrderDto> GetInstitutionOrderList(
            int institutionId,
            int? pageIndex,
            int? singletonPageCount
            )
        {
            var queryableAdmins = InstitutionRepository
                .GetAllIncluding(x => x.Admins)
                .Where(x => x.Id == institutionId)
                .SelectMany(x => x.Admins);
            var queryAble = from o in _orderRepository.GetAllIncluding(x => x.OrderItems)
                            join a in queryableAdmins on o.UserId equals a.UserId
                            orderby o.Id descending
                            select o;
            return ObjectMapper
                .Map<PaginationDataList<OrderDto>>(
                    queryAble.ToPagination(pageIndex, singletonPageCount)
                );
        }
    }
}

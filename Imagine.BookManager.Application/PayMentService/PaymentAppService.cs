using System;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.PayMent;
using Imagine.BookManager.PaymentService.AliPay;
using Imagine.BookManager.PaymentService.WeiXinPay;

namespace Imagine.BookManager.PayMentService
{
    public class PaymentAppService : BookManagerAppServiceBase, IPaymentAppService
    {
        private readonly IRepository<Payment, Int64> _paymentRepository;

        private readonly IRepository<Order, Int64> _orderRepository;

        private readonly IWeiXinPayService _iWeiXinPayService;

        private readonly IAliPayService _iAliPayService;

        public PaymentAppService(
            IRepository<Payment, Int64> paymentRepository,
            IRepository<Order, Int64> orderRepository,
            IWeiXinPayService iWeiXinPayService,
            IAliPayService iAliPayService)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _iWeiXinPayService = iWeiXinPayService;
            _iAliPayService = iAliPayService;
        }

        public WeiXinOrderResult WeiXinConfigOrder(string orderRef)
        {
            var amount = SaveGatewayOrder(PaymentGateWay.WeiXinPay, orderRef);
            return _iWeiXinPayService.GetWeiXinPayQrCode(orderRef, amount);
        }

        public string AliPayConfigOrder(string orderRef)
        {
            var amount = SaveGatewayOrder(PaymentGateWay.AliPay, orderRef);
            return _iAliPayService.GetAliPayTradeOrderResponse(orderRef, amount);
        }

        [UnitOfWork]
        public bool GetOrderPaidStatus(string xmlParamter, PaymentGateWay paymentGateWay)
        {
            PayResult result =
                paymentGateWay == PaymentGateWay.WeiXinPay ?
                _iWeiXinPayService.GetOrderPaidStatus(xmlParamter)
                : _iAliPayService.GetOrderPaidStatus(xmlParamter);
            if (result.IsSuccess == false)
                return result.IsSuccess;
            var order = _orderRepository.FirstOrDefault(x => x.OrderRef == result.OrderRef);
            order.Paid = true;
            var payment = _paymentRepository.FirstOrDefault(x => x.OrderRef == result.OrderRef);
            payment.PaymenGateway = paymentGateWay;
            payment.GatewayRef = result.GatewayRef;
            payment.Paid = true;
            payment.DatePaid = DateTime.Now;
            _orderRepository.Update(order);
            _paymentRepository.Update(payment);
            return result.IsSuccess;
        }

        public bool QueryOrderStutas(string orderRef)
        {
            var payment = _paymentRepository.FirstOrDefault(x => x.OrderRef == orderRef);
            if (payment == null)
                return false;
            return payment.Paid;
        }

        public decimal SaveGatewayOrder(PaymentGateWay paymentGateWay, string orderRef)
        {
            var order = _orderRepository.FirstOrDefault(x => x.OrderRef == orderRef);
            if (order == null || order.Paid)
            {
                throw new Exception(ExceptionInfo.OrderNotExistsOrIsPaid);
            }
            var payment = new Payment
            {
                OrderRef = orderRef,
                Amount = order.Total,
                DateCreated = DateTime.Now,
                PaymenGateway = paymentGateWay
            };
            return _paymentRepository.Insert(payment).Amount;
        }
    }
}
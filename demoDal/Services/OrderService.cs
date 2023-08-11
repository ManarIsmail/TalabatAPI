using TalabatBLL.Entities;
using TalabatBLL.Entities.Order;
using TalabatBLL.Interfaces;
using TalabatBLL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatDAL.Services
{
    public class OrderService : IOrderService
    {
        /* instead of query use unit of work */
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork,
            IBasketRepository basketRepository,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address ShippingAddress)
        {
            // get basket from repository
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //basket contain list of items  get items from product repository
            var items= new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered,productItem.Price, /*in basket*/item.Quantity);
                items.Add(orderItem);
            }

            //get delivery method 

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal

            var subtotal= items.Sum(item => item.Price * item.Quantity);

            //check if order exist

            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder=await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if(existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            //create order
            var order = new Order(buyerEmail , ShippingAddress, deliveryMethod,items,subtotal, basket.PaymentIntentId);

            _unitOfWork.Repository<Order>().Add(order);

            //save to database
            var result = await _unitOfWork.Complete();

            //check result
            if (result <= 0) return null;

            await _basketRepository.DeleteBasketAsync(basketId);
            return order;   

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec=new OrderWithItemsAndOrderingSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        } 
        
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);
        }
    }
}

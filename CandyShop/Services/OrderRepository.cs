using CandyShop.Interfaces;
using CandyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;
        public OrderRepository(AppDbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }
        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();
            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();

            var shoppingCardItems = _shoppingCart.GetShoppingCartItems();
            foreach (var shoppingCard in shoppingCardItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCard.Amount,
                    CandyId = shoppingCard.Candy.Id,
                    Price = shoppingCard.Candy.Price,
                    OrderId = order.OrderId,
                    
                };
                _appDbContext.OrderDetails.Add(orderDetail);
            }
            _appDbContext.SaveChanges();
        }
    }
}

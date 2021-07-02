using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;
        public string ShoppingCartId { get; set; }
        public List<ShoppingCardItem> ShoppingCardItems { get; set; }
        public ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();
            var cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId",cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Candy candy , int amount)
        {
            var shoppingCartItem = _appDbContext.ShoppingCardItems.SingleOrDefault(s => s.Candy.Id == candy.Id && s.ShoppingCardId == ShoppingCartId);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCardItem()
                {
                    Amount = amount,
                    ShoppingCardId = ShoppingCartId,
                    Candy = candy
                };
                _appDbContext.ShoppingCardItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveFromCart(Candy candy)
        {
            var shoppingCartItem = _appDbContext.ShoppingCardItems.SingleOrDefault(s => s.Candy.Id == candy.Id && s.ShoppingCardId == ShoppingCartId);
            var localAmount = 0;
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShoppingCardItems.Remove(shoppingCartItem);
                }
            }
            _appDbContext.SaveChanges();
            return localAmount;
        }

        public List<ShoppingCardItem> GetShoppingCartItems()
        {
            return ShoppingCardItems ?? (ShoppingCardItems = _appDbContext.ShoppingCardItems.Where(s => s.ShoppingCardId == ShoppingCartId).Include(c => c.Candy).ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext.ShoppingCardItems.Where(s => s.ShoppingCardId == ShoppingCartId).ToList();
            _appDbContext.ShoppingCardItems.RemoveRange(cartItems);
            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCardItems
                                     .Where(s => s.ShoppingCardId == ShoppingCartId)
                                     .Select(s => s.Candy.Price * s.Amount).Sum();
            return total;
        }
    }
}

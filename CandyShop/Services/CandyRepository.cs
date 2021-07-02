using CandyShop.Interfaces;
using CandyShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Services
{
    public class CandyRepository : ICandyRepository
    {
        private readonly AppDbContext _appDbContext;

        public CandyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Candy> GetAllCandy => _appDbContext.Candies.Include(c=>c.Category).ToList();

        public IEnumerable<Candy> GetCandyOnSale => _appDbContext.Candies.Where(c => c.IsOnSale).Include(c => c.Category).ToList();

        public Candy GetCandyById(int candyId)
        {
            return _appDbContext.Candies.FirstOrDefault(c => c.Id == candyId);
        }
    }
}
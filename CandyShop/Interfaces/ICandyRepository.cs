using CandyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Interfaces
{
    public interface ICandyRepository
    {
        public IEnumerable<Candy> GetAllCandy { get; }
        public IEnumerable<Candy> GetCandyOnSale { get; }

        Candy GetCandyById(int candyId);
    }
}
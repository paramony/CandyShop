using CandyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Interfaces
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> GetAllCategories { get; }
    }
}
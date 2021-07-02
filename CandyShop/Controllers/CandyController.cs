using CandyShop.Interfaces;
using CandyShop.Models;
using CandyShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Controllers
{
    public class CandyController : Controller
    {
        private readonly ICandyRepository _candyRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CandyController(ICandyRepository candyRepository, ICategoryRepository categoryRepository)
        {
            _candyRepository = candyRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult List(string category)
        {
            IEnumerable<Candy> candies;
            string categoryName = "";
            if (string.IsNullOrEmpty(category))
            {
                categoryName = "All Candy";
                candies = _candyRepository.GetAllCandy.OrderBy(c => c.Id);
            }
            else
            {
                categoryName = _categoryRepository.GetAllCategories.FirstOrDefault(c => c.Name == category)?.Name;
                candies = _candyRepository.GetAllCandy.Where(c => c.Category.Name == categoryName).OrderBy(c => c.Id);
            }
            var candyListViewModel = new CandyListViewModel();
            candyListViewModel.Candies = candies;
            candyListViewModel.CurrentCategory = categoryName;
            return View(candyListViewModel);
        }

        public IActionResult Details(int id)
        {
            var candy = _candyRepository.GetCandyById(id);
            if (candy == null)
            {
                return NotFound();
            }
            return View(candy);
        }
    }
}
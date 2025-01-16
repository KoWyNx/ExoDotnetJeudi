using Microsoft.AspNetCore.Mvc;
using TP_CAISSE.DTL;
using TP_CAISSE.Models;
using TP_CAISSE.ViewModel;
using System;
using System.Threading.Tasks;

namespace TP_CAISSE.Controllers
{
    public class CategorieController : Controller
    {
        private readonly CategorieDTL _categorieDTL;

        public CategorieController(CategorieDTL categorieDTL)
        {
            _categorieDTL = categorieDTL;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categorieDTL.GetAllCategories();
            var categoryViewModels = categories.Select(c => new CategorieViewModel(c)).ToList();
            return View(categoryViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categorie = new Categorie()
            {

            };

            return View(categorie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categorie category)
        {
            if (ModelState.IsValid)
            {
                await _categorieDTL.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categorieDTL.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Categorie category)
        {
            if (ModelState.IsValid)
            {
                await _categorieDTL.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var category = await _categorieDTL.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDetailViewModel = new CategoryDetailViewModel(category);
            return View(categoryDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categorieDTL.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

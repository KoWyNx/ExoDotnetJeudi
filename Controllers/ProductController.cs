using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using TP_CAISSE.DTL;
using TP_CAISSE.Models;
using TP_CAISSE.Services;
using TP_CAISSE.ViewModel;

namespace TP_CAISSE.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductSvc _productSvc;
        private readonly CategorieDTL _categorieDTL;

        public ProductController(ProductSvc productSvc, CategorieDTL categorieDTL)
        {
            _productSvc = productSvc;
            _categorieDTL = categorieDTL;
        }

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productSvc.GetAllProducts(); 

            return View(products);
        }


        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productSvc.GetProductById(id);
            if (product == null)
            {
                return NotFound("Produit introuvable.");
            }
            return View(product);
        }

        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categorieDTL.GetAllCategories(); 

            ViewBag.Categories = new SelectList(categories, "Primarikey", "Name"); 

            return View(new ProductViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel product, Guid categoryId)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categorieDTL.GetAllCategories();
                ViewBag.Categories = new SelectList(categories, "Primarikey", "Name");
                return View(product);
            }

            try
            {
                await _productSvc.AddProduct(product, categoryId);
                return RedirectToAction("GetAllProducts");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var categories = await _categorieDTL.GetAllCategories();
                ViewBag.Categories = new SelectList(categories, "Primarikey", "Name");
                return View(product);
            }
        }




        public async Task<IActionResult> EditProduct(Guid id)
        {
            var productViewModel = await _productSvc.GetProductById(id);

            if (productViewModel == null)
            {
                return NotFound("Produit introuvable.");
            }

            var categories = await _categorieDTL.GetAllCategories();

            ViewBag.Categories = new SelectList(categories, "Primarikey", "Name", productViewModel.CategoryId);

            return View(productViewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel product, Guid categoryId)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categorieDTL.GetAllCategories();
                ViewBag.Categories = new SelectList(categories, "Primarikey", "Name", categoryId); 
                return View(product);
            }

            try
            {
                await _productSvc.UpdateProduct(product, categoryId);
                return RedirectToAction("GetAllProducts");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var categories = await _categorieDTL.GetAllCategories();
                ViewBag.Categories = new SelectList(categories, "Primarikey", "Name", categoryId);
                return View(product);
            }
        }
    }
}

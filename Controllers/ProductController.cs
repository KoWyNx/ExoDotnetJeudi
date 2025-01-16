using Microsoft.AspNetCore.Mvc;
using TP_CAISSE.DTL;
using TP_CAISSE.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TP_CAISSE.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDTL _productDTL;

        public ProductController(ProductDTL productDTL)
        {
            _productDTL = productDTL;  
        }

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productDTL.GetAllProducts();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return View(products);
        }

        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productDTL.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult AddProduct()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productDTL.AddAsync(product);
                return RedirectToAction("GetAllProducts");
            }
            return View(product);
        }

        public async Task<IActionResult> EditProduct(Guid id)
        {
            var product = await _productDTL.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productDTL.UpdateAsync(product);
                return RedirectToAction("GetAllProducts");
            }
            return View(product);
        }
    }
}

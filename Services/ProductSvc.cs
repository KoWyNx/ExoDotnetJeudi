using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using TP_CAISSE.DTL;
using TP_CAISSE.Models;
using TP_CAISSE.ViewModel;

namespace TP_CAISSE.Services
{
    public class ProductSvc
    {
        private readonly ProductDTL _productDTL;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CategorieDTL _categorieDTL;

        public ProductSvc(IServiceProvider provider, IHttpContextAccessor httpContextAccessor)
        {
            _productDTL = provider.GetRequiredService<ProductDTL>();
            _httpContextAccessor = httpContextAccessor;
            _categorieDTL = provider.GetRequiredService<CategorieDTL>();
        }




        public async Task<List<ProductViewModel>> GetAllProducts()
        {
            var products = await _productDTL.GetAllProducts();

            return products;
        }



        public async Task<ProductViewModel> GetProductById(Guid id)
        {
            var product = await _productDTL.GetByIdAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Le produit spécifié n'existe pas.");
            }


            return product;
        }

        public async Task<List<ProductViewModel>> GetProductsByCategorie(Guid fkCategorie, List<ProductViewModel> product)
        {
            var products = await _categorieDTL.GetProductsByCategorie(fkCategorie);

            if (product == null || !product.Any())
            {
                throw new KeyNotFoundException("Aucun produit associé");
            }
            return product;
        }







        public async Task<Product> AddProduct(ProductViewModel productDisplayModel, Guid categoryId)
        {
            if (productDisplayModel == null)
            {
                throw new ArgumentNullException(nameof(productDisplayModel), "Le modèle de produit ne peut pas être null.");
            }

            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("L'identifiant de la catégorie est invalide.", nameof(categoryId));
            }

            var category = await _categorieDTL.GetCategoryById(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("La catégorie spécifiée n'existe pas.");
            }

            var newProduct = new Product
            {
                Primarikey = Guid.NewGuid(),
                Name = productDisplayModel.Name,
                Description = productDisplayModel.Description,
                Stock = productDisplayModel.Stock,
                Price = productDisplayModel.Price
            };

            newProduct.FkCategories.Add(category);

            await _productDTL.Context.Products.AddAsync(newProduct);
            await _productDTL.Context.SaveChangesAsync();

            return newProduct;
        }

        public async Task<Product> UpdateProduct(ProductViewModel productDisplayModel, Guid categoryId)
        {
            if (productDisplayModel == null)
            {
                throw new ArgumentNullException(nameof(productDisplayModel), "Le modèle de produit ne peut pas être null.");
            }

            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("L'identifiant de la catégorie est invalide.", nameof(categoryId));
            }

            var existingProduct = await _productDTL.GetProductWithCategoriesAsync(productDisplayModel.PrimaryKey);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Le produit spécifié n'existe pas.");
            }

            var category = await _categorieDTL.GetCategoryById(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("La catégorie spécifiée n'existe pas.");
            }

            existingProduct.Name = productDisplayModel.Name;
            existingProduct.Description = productDisplayModel.Description;
            existingProduct.Stock = productDisplayModel.Stock;
            existingProduct.Price = productDisplayModel.Price;

         
            existingProduct.FkCategories.Clear(); 
            existingProduct.FkCategories.Add(category); 

            await _productDTL.UpdateAsync(existingProduct);

            return existingProduct;
        }









    }
}

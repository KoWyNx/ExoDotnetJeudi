using Microsoft.EntityFrameworkCore;
using TP_CAISSE.Models;
using TP_CAISSE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_CAISSE.DTL
{
    public class ProductDTL : GenericRepository<Product>
    {
        public ProductDTL(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<List<ProductViewModel>> GetAllProducts()
        {
            return await Context.Products
                .Include(p => p.FkCategories) 
                .Select(m => new ProductViewModel(m))
                .ToListAsync();
        }

        public async Task<ProductViewModel?> GetByIdAsync(Guid id)
        {
            return await Context.Products
                .Where(product => product.Primarikey == id)
                .Select(product => new ProductViewModel(product)) 
                .FirstOrDefaultAsync(); 
        }
        public async Task<Product?> GetProductWithCategoriesAsync(Guid id)
        {
            return await Context.Products
                .Include(p => p.FkCategories) 
                .FirstOrDefaultAsync(p => p.Primarikey == id);
        }


        public async Task AddAsync(Product product)
        {
            await Context.Products.AddAsync(product);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            Context.Products.Update(product);
            await Context.SaveChangesAsync();
        }
    }
}
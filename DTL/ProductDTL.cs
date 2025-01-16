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
                .Select(m => new ProductViewModel(m))
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await Context.Products
                .FirstOrDefaultAsync(product => product.Primarikey == id);  
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
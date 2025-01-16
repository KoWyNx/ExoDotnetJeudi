using Microsoft.EntityFrameworkCore;
using TP_CAISSE.Models;
using TP_CAISSE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_CAISSE.DTL
{
    public class CategorieDTL
    {
        private readonly Context.MyDbContext _context;

        public CategorieDTL(Context.MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categorie>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categorie> GetCategoryById(Guid id)
        {
            return await _context.Categories
                .Include(c => c.FkProducts)
                .FirstOrDefaultAsync(c => c.Primarikey == id);
        }

        public async Task AddCategory(Categorie categorie)
        {
            await _context.Categories.AddAsync(categorie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Categorie categorie)
        {
            _context.Categories.Update(categorie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
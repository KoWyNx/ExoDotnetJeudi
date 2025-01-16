using TP_CAISSE.Models;
using TP_CAISSE.ViewModel;

namespace TP_CAISSE.ViewModel
{
    public class CategoryDetailViewModel
    {
        public Guid Primarikey { get; set; }
        public string Name { get; set; }
        public List<ProductViewModel> Products { get; set; }

        public CategoryDetailViewModel(Categorie categorie)
        {
            Primarikey = categorie.Primarikey;
            Name = categorie.Name;
            Products = categorie.FkProducts.Select(p => new ProductViewModel(p)).ToList();
        }
    }
}
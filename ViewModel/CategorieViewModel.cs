using TP_CAISSE.Models;

namespace TP_CAISSE.ViewModel
{
    public class CategorieViewModel
    {
        public Guid Primarikey { get; set; }
        public string Name { get; set; }
        public List<ProductViewModel> Products { get; set; }


        public CategorieViewModel(Categorie categorie)
        {
            ParseFromPocs(categorie);
        }

        public void ParseFromPocs(Categorie categorie)
        {
            if (categorie == null) return;

            Primarikey = categorie.Primarikey;
            Name = categorie.Name;
            Products = categorie.FkProducts.Select(p => new ProductViewModel(p)).ToList();

        }
    }
}
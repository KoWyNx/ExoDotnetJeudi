using TP_CAISSE.Models;

namespace TP_CAISSE.ViewModel
{
    public class ProductViewModel
    {
        public Guid Primarikey { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Stock { get; set; }

        public ProductViewModel() { }

        public ProductViewModel(Product produit)
        {
            ParseFromPocs(produit);
        }

        public void ParseFromPocs(Product produit)
        {
            if (produit == null) return;

            Name = produit.Name; 
            Primarikey = produit.Primarikey;
            Description = produit.Description; 
            Price = produit.Price ?? 0; 
            Stock = produit.Stock ?? 0; 
        }
    }
}
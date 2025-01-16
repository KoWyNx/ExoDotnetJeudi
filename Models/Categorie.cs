using System;
using System.Collections.Generic;

namespace TP_CAISSE.Models;

public partial class Categorie
{
    public Guid Primarikey { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> FkProducts { get; set; } = new List<Product>();
}

using System;
using System.Collections.Generic;

namespace TP_CAISSE.Models;

public partial class Product
{
    public Guid Primarikey { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Stock { get; set; }

    public byte[]? Image { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<Categorie> FkCategories { get; set; } = new List<Categorie>();
}

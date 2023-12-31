using Organico.Library.Model;
using System;
using System.Collections.Generic;

namespace Organico.Library.Data;

// Represents the e-commerce base class with sample product list
public class BaseECommerceData
{
    private static List<Product> _products = new List<Product>
        {
            new Product("1", "🍇", "Uva roxa (cacho 100g)", 15m),
            new Product("2", "🍈", "Melão (un)", 3.50m),
            new Product("3", "🍉", "Melancia (un)", 5.50m),
            new Product("4", "🍊", "Tangerina (kg)", 3.50m),
            new Product("5", "🍋", "Limão (kg)", 3.50m),
            new Product("6", "🍌", "Banana (12 un)", 3.50m),
            new Product("7", "🍍", "Abacaxi (un)", 3.50m),
            new Product("8", "🥭", "Manga (kg)", 4.50m),
            new Product("9", "🍎", "Maçã Vermelha (kg)", 3.50m),
            new Product("10", "🍏", "Maçã Verde (kg)", 6.50m),
            new Product("11", "🍐", "Pera (kg)", 3.50m),
            new Product("12", "🍑", "Pêssego (kg)", 3.50m),
            new Product("13", "🍒", "Cerejas (kg)", 3.50m),
            new Product("14", "🍓", "Morango orgânico (cx c/ 20)", 13m),
            new Product("15", "🥝", "Kiwi (kg)", 7.50m),
            new Product("16", "🍅", "Tomate (kg)", 2.50m),
            new Product("17", "🥥", "Coco (un)", 4.50m)
        };

    public static int MaxOrderId { get; private set; } = 0;

    // Initializes the e-commerce with sample data
    public List<Product> GetProductList()
    {
        return _products;
    }

    public Product? GetProduct(string productId)
    {
        return _products.Where(p => p.Id == productId).SingleOrDefault();
    }
}

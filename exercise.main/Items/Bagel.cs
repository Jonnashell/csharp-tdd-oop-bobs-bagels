using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Items
{
    public class Bagel : IInventoryItem
    {
        private double _totalPrice;
        public string SKU { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Variant { get; set; }
        public List<Filling> Fillings { get; set; }

        public Bagel(string sku, double price, string name, string variant, List<Filling>? fillings = null)
        {
            // Initialize empty list of fillings, if none was provided
            fillings ??= new List<Filling>();

            SKU = sku;
            Price = price;
            Name = name;
            Variant = variant;
            Fillings = fillings;
        }

        public void Add(Filling filling)
        {
            Fillings.Add(filling);
            Price += filling.Price;
        }
    }
}

using exercise.main.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main
{
    public class Basket
    {

        private Dictionary<string, IInventoryItem> _inventoryItems = new Dictionary<string, IInventoryItem>
        {
            { "BGLO", new Bagel("BGLO", 0.49, "Bagel", "Onion") },
            { "BGLP", new Bagel("BGLP", 0.39, "Bagel", "Plain") },
            { "BGLE", new Bagel("BGLE", 0.49, "Bagel", "Everything") },
            { "BGLS", new Bagel("BGLS", 0.49, "Bagel", "Sesame") },
            { "COFB", new Coffee("COFB", 0.99, "Coffee", "Black") },
            { "COFW", new Coffee("COFW", 1.19, "Coffee", "White") },
            { "COFC", new Coffee("COFC", 1.29, "Coffee", "Capuccino") },
            { "COFL", new Coffee("COFL", 1.29, "Coffee", "Latte") },
            { "FILB", new Filling("FILB", 0.12, "Filling", "Bacon") },
            { "FILE", new Filling("FILE", 0.12, "Filling", "Egg") },
            { "FILC", new Filling("FILC", 0.12, "Filling", "Cheese") },
            { "FILX", new Filling("FILX", 0.12, "Filling", "Cream Cheese") },
            { "FILS", new Filling("FILS", 0.12, "Filling", "Smoked Salmon") },
            { "FILH", new Filling("FILH", 0.12, "Filling", "Ham") }
        };
        private int _capacity = 5;
        private List<IInventoryItem> _items = new List<IInventoryItem>();

        private bool _confirmInStock(IInventoryItem item)
        {
            throw new NotImplementedException();
        }

        public bool Add(IInventoryItem item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IInventoryItem item)
        {
            throw new NotImplementedException();
        }

        public void ChangeCapacity(int newCapacity)
        {
            throw new NotImplementedException();
        }

        public double TotalCost { get { return _items.Sum(i => i.Price); } }
        public List<IInventoryItem> Items { get { return _items; } }
        public int Capacity { get { return _capacity; } }
    }
}

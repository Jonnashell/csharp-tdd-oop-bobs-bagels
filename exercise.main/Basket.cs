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
        private int _capacity = 50;
        private List<IInventoryItem> _items = new List<IInventoryItem>();

        private bool _confirmInStock(IInventoryItem diff_item)
        {
            if (_inventoryItems.ContainsKey(diff_item.SKU))
            {
                IInventoryItem ref_item = _inventoryItems[diff_item.SKU];
                if (ref_item.SKU == diff_item.SKU &&
                    ref_item.Price == diff_item.Price &&
                    ref_item.Name == diff_item.Name &&
                    ref_item.Variant == diff_item.Variant)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Add(IInventoryItem item)
        {
            if (_items.Count >= _capacity)
            {
                Console.WriteLine("Basket is full!");
                return false;
            }
            if (_confirmInStock(item))
            {
                _items.Add(item);
                return true;
            }
            return false;
        }

        public bool Remove(IInventoryItem item)
        {
            return _items.Remove(item);
        }

        public void ChangeCapacity(int newCapacity)
        {
            _capacity = newCapacity;
        }

        public double GetItemPrice(string sku)
        {
            if (_inventoryItems.ContainsKey(sku))
            {
                Console.WriteLine($"Price for SKU '{sku}' ({_inventoryItems[sku].Variant}): {_inventoryItems[sku].Price}");
                return _inventoryItems[sku].Price;
            }
            else return -1;
        }

        public string GetFillingPrices()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in _inventoryItems)
            {
                if (kvp.Value.Name == "Filling")
                {
                    sb.Append($"Variant: {kvp.Value.Variant}, Price: {kvp.Value.Price}\n");
                    //Console.WriteLine($"Variant: {kvp.Value.Variant}, Price: {kvp.Value.Price}");
                }
            }
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        public double GetDiscountedCosts()
        {
            /*
            double totalPrice = 0
            For each bagel SKU:
                bigDiscount = bagel.Count // 12
                IF bigDiscount > 0:
                    totalPrice += 3.99 * bigDiscount
                
                smallDiscount = 
                
                IF (bagel.Count % 12 == 0):
                    bagel.Count / 12
                    totalPrice = 3.99
                IF (bagel.Count % 6 == 0 || bagel.Count % 12 == 0):
                    
                

            */
            throw new NotImplementedException();
        }

        // Extension 2
        public string PrintReceipt()
        {
            throw new NotImplementedException();
        }

        public double TotalCost { get { return _items.Sum(item => item.Price); } }
        public List<IInventoryItem> Items { get { return _items; } }
        public int Capacity { get { return _capacity; } }
    }
}

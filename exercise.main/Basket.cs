using exercise.main.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main
{
    public class Basket
    {
        // Easy lookup for items in stock
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
                }
            }
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        private Dictionary<string, int> GetBagelsDict()
        {
            Dictionary<string, int> bagelsDict = new Dictionary<string, int>();
            foreach (var item in _items)
            {
                if (item is Bagel bagel)
                {
                    // Add key if doesn't exist
                    if (!bagelsDict.ContainsKey(item.SKU))
                    {
                        // Set count to 1
                        bagelsDict[item.SKU] = 0;
                    }
                    // Increment count by 1
                    bagelsDict[item.SKU] += 1;
                }
            }
            return bagelsDict;
        }

        public double GetDiscountedCosts()
        {
            // Bagel_type, Bagel_type_amount, Bagel_type_price
            /*
            double totalPrice = 0

            bigDiscount = Bagel_type_amount / 12
            remainderBig = Bagel_type_amount % 12

            smallDiscount = remainderBig / 6
            remainderSmall = remainderBig % 6

            totalPrice += (3.99 * bigDiscount)
            totalPrice += (2.49 * smallDiscount) + (remainderSmall * Bagel_type_price)
            */
            Dictionary<string, int> bagelsDict = GetBagelsDict();
            List<(string, int, double)> receiptList = new List<(string, int, double)> ();  
            double totalPrice = 0;
            foreach (var kvp in bagelsDict)
            {
                // Define variables
                string bagel_type = kvp.Key;
                int bagel_type_amount = kvp.Value;
                double bagel_type_price = _inventoryItems[kvp.Key].Price;

                // Initialize price
                double price = 0;

                // Get discount price for 12x bagels
                int bigDiscount = bagel_type_amount / 12;
                int remainderBig = bagel_type_amount % 12;

                double bigPrice = (3.99 * bigDiscount);

                // Add discounted price to list, if exists
                if (bigDiscount > 0)
                {
                    receiptList.Add((bagel_type, bigDiscount * 12, bigPrice));
                }

                // Get discount price for 6x bagels
                int smallDiscount = remainderBig / 6;
                int remainderSmall = remainderBig % 6;

                double smallPrice = (2.49 * smallDiscount);

                // Add discounted price to list, if exists
                if (smallDiscount > 0)
                {
                    receiptList.Add((bagel_type, smallDiscount * 6, smallPrice));
                }

                double remainderPrice = (bagel_type_price * remainderSmall);

                if (remainderSmall > 0)
                {
                    receiptList.Add((bagel_type, remainderSmall, remainderPrice));
                }

                price = bigPrice + smallPrice + remainderPrice;
                totalPrice += price;
            }
            return totalPrice;
        }

        // Extension 2
        public string PrintReceipt()
        {
            double total_cost = 5.55;
            // double total_cost = GetTotalCosts();
            StringBuilder sb = new StringBuilder();

            sb.Insert(0, $"\t~~~ Bob's Bagels ~~~\n\n\t{DateTime.Now.ToString()}\n\n---------------------------\n\n");
            foreach (var item in _items)
            {
                if (item is Bagel bagel)
                {
                    sb.Append($"{item.Variant} Bagel\t\t'X'\t£{item.Price}\n".Replace(",", "."));

                    foreach (Filling f in bagel.Fillings)
                    {
                        sb.Append($"{f.Variant} Filling\t'X'\t£{f.Price}\n".Replace(",","."));
                    }
                }
                else if (item is Coffee coffee)
                {
                    sb.Append($"{item.Variant} Coffee\t'X'\t£{item.Price}\n");
                }
            }

            sb.Append($"\n---------------------------\n");
            sb.Append($"Total\t\t\t\t£{total_cost}\n\n".Replace(",", "."));

            sb.Append($"\t\tThank you\n\t  for your order!");
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        public double TotalCost { get { return _items.Sum(item => item.Price); } }
        public List<IInventoryItem> Items { get { return _items; } }
        public int Capacity { get { return _capacity; } }
    }
}

using exercise.main.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        private double _totalCosts;
        private List<(string, int, double, string, double)> _receiptList;
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

        private Dictionary<string, int> _itemDictCount { get { return _items.GroupBy(item => item.SKU).ToDictionary(group => group.Key, group => group.Count()); } }

        private List<(string, int, double, string, double)> ReceiptHelp(string sku, int discountNum, int discountType, double price, string variant, double singlePrice)
        {
            List<(string, int, double, string, double)> result = new List<(string, int, double, string, double)>();
            for (int i = 0; i < discountNum; i++)
            {
                double saved = price - (singlePrice * discountType);
                result.Add((sku, discountNum * discountType, price, variant, saved));
            }
            return result;
        }

        public double GetTotalCosts()
        {
            List<(string, int, double, string, double)> receiptList = new List<(string, int, double, string, double)>();
            double totalPrice = 0;
            List<Bagel> remainingBagels = new List<Bagel>();
            var groupedItems = _items.GroupBy(item => item.SKU).OrderBy(group => group.Key.StartsWith("BGL") ? 0 : 1).ToList();
            foreach (var group in groupedItems)
            {
                double price = 0;
                foreach (IInventoryItem item in group)
                {
                    if (item is Bagel bagel)
                    {
                        // Only get discount price for first occurence of SKU
                        if (price <= 0)
                        {
                            (double discountPrice, int bigDiscount, int smallDiscount, int remainders) = GetDiscountedCosts(item.SKU);
                            price += discountPrice;

                            // Add remaining bagels for later coffee deals
                            for (int i = 0; i < remainders; i++)
                            {
                                remainingBagels.Add(bagel);
                            }

                            // Receipt stuff (hard coded discount price)
                            receiptList.AddRange(ReceiptHelp(item.SKU, bigDiscount, 12, 3.99, item.Variant, item.Price));
                            receiptList.AddRange(ReceiptHelp(item.SKU, smallDiscount, 6, 2.49, item.Variant, item.Price));
                        }

                        // Add filling price for all bagels
                        foreach (Filling filling in bagel.Fillings)
                        {
                            receiptList.Add((filling.SKU, 1, filling.Price, filling.Variant, 0));
                            price += filling.Price;
                        }
                    }

                    if (item is Coffee coffee)
                    {
                        // If possible coffee deal:
                        if (remainingBagels.Count > 0)
                        {
                            // Remove most expensive bagel and make a coffee deal with it.
                            Bagel maxBagel = remainingBagels.MaxBy(item => item.Price); // Could do Min() for profit I guess...
                            remainingBagels.Remove(maxBagel);

                            // Hard coded discount price
                            receiptList.Add(("COFB", 1, 1.25, $"Bgl&Cfe", 1.25 - (coffee.Price + maxBagel.Price)));
                            totalPrice += 1.25;
                        }
                        else
                        {
                            receiptList.Add((coffee.SKU, 1, coffee.Price, coffee.Variant, 0));
                            totalPrice += coffee.Price;
                        }
                    }
                }
                totalPrice += price;
            }
            // Add remaining bagels not matched with a discounted offer
            for (int i = 0; i < remainingBagels.Count; i++)
            {
                totalPrice += remainingBagels[i].Price;
                receiptList.Add((remainingBagels[i].SKU, 1, remainingBagels[i].Price, remainingBagels[i].Variant, 0));
            }
            // Update private receipt list
            _receiptList = receiptList;
            _totalCosts = totalPrice;
            return totalPrice;
        }

        public (double, int, int, int) GetDiscountedCosts(string sku)
        {
            // Define variables
            string bagel_type = sku;
            int bagel_type_amount = _itemDictCount[sku];
            double bagel_type_price = _inventoryItems[sku].Price;

            // Initialize price
            double price = 0;

            // Get discount price for 12x bagels
            int bigDiscount = bagel_type_amount / 12;
            int remainderBig = bagel_type_amount % 12;

            double bigPrice = (3.99 * bigDiscount);

            // Get discount price for 6x bagels
            int smallDiscount = remainderBig / 6;
            int remainderSmall = remainderBig % 6;

            double smallPrice = (2.49 * smallDiscount);
            double remainderPrice = remainderSmall * bagel_type_price;

            price = bigPrice + smallPrice;
            
            return (price, bigDiscount, smallDiscount, remainderSmall);
        }

        // Extension 2
        public string PrintReceipt()
        {
            // Run GetTotalCosts to fill _receiptList and _totalCosts
            if (_receiptList is null) GetTotalCosts();
            double totalSaved = 0;
            StringBuilder sb = new StringBuilder();
            sb.Insert(0, $"\t~~~ Bob's Bagels ~~~\n\n\t{DateTime.Now.ToString()}\n\n---------------------------------\n\n");

            for (int i = 0; i < _receiptList.Count; i++)
            {
                string itemType = "Bagel";
                (string sku, int quantity, double price, string variant, double saved) = _receiptList[i];
                if (sku.StartsWith("C"))
                {
                    itemType = "Coffee";
                }
                else if (sku.StartsWith("F"))
                {
                    itemType = "Filling";
                }

                //sb.Append($"{variant} {itemType}{quantity}\t£{price}".Replace(",", "."));
                sb.AppendFormat("{0, -10}{1,-8}{2,-4}{3,6}\n", variant, itemType, quantity, $"£{price}");
                if (saved != 0)
                {
                    totalSaved += Math.Abs(saved);
                    sb.AppendFormat("{0, 29}", $"(-€{Math.Abs(Math.Round(saved, 2))})");
                    //sb.Append($"\n\t\t\t\t  (-£{Math.Abs(Math.Round(saved, 2))})".Replace(",", "."));
                }

                sb.Append("\n");
            }

            sb.Append($"\n---------------------------------\n");
            //sb.Append($"Total\t\t\t\t£{Math.Round(_totalCosts, 2)}\n\n".Replace(",", "."));
            sb.AppendFormat("Total{0, 25}", $"£{Math.Round(_totalCosts, 2)}\n\n");

            sb.Append($"You have saved a total of £{Math.Round(totalSaved,2)}\n\t\ton this shop\n\n");

            sb.Append($"\t\tThank you\n\t  for your order!");
            Console.WriteLine(sb.ToString().Replace(",","."));
            return sb.ToString();
        }

        public double TotalCost { get { return _items.Sum(item => item.Price); } }
        public List<IInventoryItem> Items { get { return _items; } }
        public int Capacity { get { return _capacity; } }
    }
}

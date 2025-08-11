using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main
{
    public interface IInventoryItem
    {
        string SKU { get; }
        string Name { get; }
        double Price { get; }
        string Variant { get; }
    }
}

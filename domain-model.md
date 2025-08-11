| Classes			| Methods/Properties                                | Scenario							| Outputs          |
|-------------------|---------------------------------------------------|-----------------------------------|------------------
|Basket.cs			|Add(IInventoryItem item)						|Add an item to the basket			|bool
|Basket.cs			|Remove(IInventoryItem item)					|Remove an item from the basket		|bool
|Basket.cs			|ChangeCapacity(int TaskId)						|Change status for a task			|void
|IInventoryItem.cs	|string SKU										|SKU propety for all item types		|string
|IInventoryItem.cs	|double Price									|Price propety for all item types	|double
|IInventoryItem.cs	|string Name									|Name propety for all item types	|string
|IInventoryItem.cs	|string variant									|Variant propety for all item types	|string
|Coffee.cs			|string SKU										|SKU propety for all item types		|string
|Coffee.cs			|double Price									|Price propety for all item types	|double
|Coffee.cs			|string Name									|Name propety for all item types	|string
|Coffee.cs			|string variant									|Variant propety for all item types	|string
|Filling.cs			|string SKU										|SKU propety for all item types		|string
|Filling.cs			|double Price									|Price propety for all item types	|double
|Filling.cs			|string Name									|Name propety for all item types	|string
|Filling.cs			|string variant									|Variant propety for all item types	|string
|Bagel.cs			|string SKU										|SKU propety for all item types		|string
|Bagel.cs			|double Price									|Price propety for all item types	|double
|Bagel.cs			|string Name									|Name propety for all item types	|string
|Bagel.cs			|string variant									|Variant propety for all item types	|string
|Bagel.cs			|List\<Filling\> Fillings						|List of fillings for each bagel	|List\<Filling\>
|Bagel.cs			|Add(Filling filling)							|Add fillings to each bagel order	|bool
using exercise.main;
using exercise.main.Items;

namespace exercise.tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AddItemToBasket()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");

        // Act
        basket.Add(bagel);

        // Assert
        Assert.That(basket.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public void FullBasketTest()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");

        // Act
        basket.ChangeCapacity(0);
        basket.Add(bagel);

        // Assert
        Assert.That(basket.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public void RemoveItemFromBasket()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");

        // Act
        basket.Add(bagel);
        basket.Remove(bagel);

        // Assert
        Assert.That(basket.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public void RemoveWrongItemFromBasket()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLP", price: 0.39, name: "Bagel", variant: "Plain");

        // Act
        bool result = basket.Remove(bagel);

        // Assert
        Assert.That(result == false);
    }

    [Test]
    public void ChangeCapacityTest()
    {
        // Arrange
        Basket basket = new Basket();

        // Act
        int newCapacity = 10;
        basket.ChangeCapacity(newCapacity);

        // Assert
        Assert.That(basket.Capacity, Is.EqualTo(newCapacity));
    }

    [Test]
    public void TotalCostTest()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Bagel bagel2 = new Bagel(sku: "BGLP", price: 0.39, name: "Bagel", variant: "Plain");

        // Act
        basket.Add(bagel);
        basket.Add(bagel2);

        // Assert
        Assert.That(basket.TotalCost, Is.EqualTo(bagel.Price + bagel2.Price));
    }

    [Test]
    public void InStockTest()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Bagel bagel2 = new Bagel(sku: "BGLPX", price: 0.39, name: "Bagel", variant: "Plain");

        // Act
        bool result1 = basket.Add(bagel);
        bool result2 = basket.Add(bagel2); // invalid SKU

        // Assert
        Assert.That(result1 == true && result2 == false);
    }
}
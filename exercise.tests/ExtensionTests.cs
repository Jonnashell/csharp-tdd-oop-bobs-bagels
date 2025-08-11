using exercise.main;
using exercise.main.Items;

namespace exercise.tests;

public class ExtensionTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SimpleBagelDiscountTest()
    {
        // Arrange
        Basket basket1 = new Basket();
        Basket basket2 = new Basket();

        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");

        // Act
        for (int i = 0; i <= 6; i++)
        {
            basket1.Add(bagel);
        }

        for (int i = 0; i <= 12; i++)
        {
            basket2.Add(bagel);
        }


        // Assert
        //Assert.That(basket1.TotalCost == 2.49 && basket2.TotalCost == 3.99);
        Assert.That(basket1.TotalCost, Is.EqualTo(2.49));
        Assert.That(basket2.TotalCost, Is.EqualTo(3.99));
    }

    [Test]
    public void PrintReceiptTest()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Filling filling = new Filling(sku: "FILB", price: 0.12, name: "Filling", variant: "Bacon");

        // Act
        string receipt = basket.PrintReceipt();

        // Assert
        Assert.That(receipt.Length, Is.GreaterThan(1)); // Non-empty string
    }
}
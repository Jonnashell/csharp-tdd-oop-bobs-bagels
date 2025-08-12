using exercise.main;
using exercise.main.Items;
using System;

namespace exercise.tests;

public class ExtensionTests
{

    [Test]
    public void SimpleBagelDiscountTest()
    {
        // Arrange
        Basket basket1 = new Basket();
        Basket basket2 = new Basket();

        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Filling filling = new Filling(sku: "FILB", price: 0.12, name: "Filling", variant: "Bacon");
        bagel.Add(filling);

        // Act
        for (int i = 0; i < 6; i++)
        {
            basket1.Add(bagel);
        }

        double result1 = Math.Round(2.49 + (0.12 * 6), 2);

        Bagel bagel2 = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");

        for (int i = 0; i < 12; i++)
        {
            basket2.Add(bagel2);
        }

        double result2 = Math.Round(3.99, 2);


        // Assert
        Assert.That(Math.Round(basket1.GetTotalCosts(), 2), Is.EqualTo(result1));
        Assert.That(Math.Round(basket2.GetTotalCosts(), 2), Is.EqualTo(result2));
    }

    [Test]
    public void BagelAndCoffeeDiscountTest()
    {
        // Example: 1 deal of 6 bagels, 1 deal of coffe + bagel
        // Arrange
        Basket basket1 = new Basket();

        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Coffee coffee = new Coffee(sku: "COFB", price: 0.99, name: "Coffee", variant: "Black");

        // Act
        basket1.Add(coffee);
        for (int i = 0; i < 7; i++)
        {
            basket1.Add(bagel);
        }

        double result1 = Math.Round(2.49 + 1.25, 2);

        // Assert
        Assert.That(Math.Round(basket1.GetTotalCosts(), 2), Is.EqualTo(result1));
    }

    [Test]
    public void BagelAndCoffeeDiscountExtraTest()
    {
        // Example: 1 deal of 6 bagels, 1 deal of coffe + bagel
        // Arrange
        Basket basket1 = new Basket();

        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Coffee coffee = new Coffee(sku: "COFB", price: 0.99, name: "Coffee", variant: "Black");

        // Act
        basket1.Add(coffee);
        for (int i = 0; i < 15; i++)
        {
            basket1.Add(bagel);
        }

        double result1 = Math.Round(3.99 + 1.25 + (0.49 * 2), 2);

        // Assert
        Assert.That(Math.Round(basket1.GetTotalCosts(), 2), Is.EqualTo(result1));
    }

    [Test]
    public void PrintReceiptTest()
    {
        // Arrange
        Basket basket = new Basket();
        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Filling filling = new Filling(sku: "FILB", price: 0.12, name: "Filling", variant: "Bacon");

        // Act
        bagel.Add(filling);
        basket.Add(bagel);
        string receipt = basket.PrintReceipt();

        // Assert
        Assert.That(receipt.Length, Is.GreaterThan(1)); // Non-empty string
    }

    [Test]
    public void PrintReceiptExtraTest()
    {
        // Example: 1 deal of 6 bagels, 1 deal of coffe + bagel
        // Arrange
        Basket basket1 = new Basket();

        Bagel bagel = new Bagel(sku: "BGLO", price: 0.49, name: "Bagel", variant: "Onion");
        Bagel bagel2 = new Bagel(sku: "BGLS", price: 0.49, name: "Bagel", variant: "Sesame");
        Coffee coffee = new Coffee(sku: "COFB", price: 0.99, name: "Coffee", variant: "Black");

        // Act
        basket1.Add(coffee);
        for (int i = 0; i < 13; i++)
        {
            basket1.Add(bagel);
        }

        for (int i = 0; i < 7; i++)
        {
            basket1.Add(bagel2);
        }

        string receipt = basket1.PrintReceipt();

        // Assert
        Assert.That(receipt.Length, Is.GreaterThan(1)); // Non-empty string
    }
}
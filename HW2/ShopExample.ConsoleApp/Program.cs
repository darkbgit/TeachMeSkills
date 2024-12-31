using ShopExample.Core;

var balance = 100d;

var random = new Random();

var products = new List<Product>(){
            new()
            {
                Name = "Tomato",
                Price = random.Next(100)
            },
            new()
            {
                Name = "Cucumber",
                Price = random.Next(100)
            },
            new()
            {
                Name = "Watermelon",
                Price = random.Next(100)
            },
            new()
            {
                Name = "Cherry",
                Price = random.Next(100)
            }};

var shop = new Shop(products);


Console.WriteLine("Welcome");
while (true)
{
    Console.WriteLine($"Your balance {balance} USD");
    Console.WriteLine("Available products:");
    Console.WriteLine(shop.ToString());
    Console.WriteLine("Choose product number to buy or 0 for exit:");

    int index;
    while (true)
    {
        if (!int.TryParse(Console.ReadLine(), out var id) || id < 0)
        {
            Console.WriteLine("Wrong input");
        }
        else
        {
            index = id;
            break;
        }
    }

    if (index == 0)
    {
        Console.WriteLine("Bye");
        break;
    }

    var product = shop.GetProduct(index - 1);
    if (product == null)
    {
        Console.WriteLine($"There is no product with number {index} in the shop. Choose right number.");
        Console.WriteLine("");
        continue;
    }
    if (balance < product.Price)
    {
        Console.WriteLine($"You don't have enough money to buy this product, choose another one.");
        Console.WriteLine("");
        continue;
    }
    shop.BuyProduct(product);
    balance -= product.Price;
    Console.WriteLine($"You bought {product.Name} for {product.Price} USD");
    Console.WriteLine("");
}


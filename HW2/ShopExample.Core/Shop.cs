namespace ShopExample.Core;

public class Shop(List<Product> products)
{
    private readonly List<Product> _products = products;

    public Product? GetProduct(int index)
    {
        return _products.ElementAtOrDefault(index);
    }

    public void BuyProduct(Product product)
    {
        _products.Remove(product);
    }

    public override string ToString()
    {
        if (_products.Count() == 0)
            return "This shop is empty.";

        return string.Join(Environment.NewLine, _products.Select((p, i) => $"{i + 1}. {p.Name} {p.Price} USD"));
    }
}

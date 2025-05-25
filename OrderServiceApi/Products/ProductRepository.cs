namespace OrderServiceApi.Products;

public class ProductRepository
{
    private readonly string[] _products =
    [
        "IP14P", // iPhone 14 Pro
        "MB23A", // MacBook Air 2023
        "PS5C",  // PlayStation 5 Console
        "GC20",  // Gift Card $20
        "HD1T",  // 1TB Hard Drive
        "TV55",  // 55-inch TV
        "WMN1",  // Women's Nike Shoes
        "TSH1",  // T-Shirt Size M
        "BAGX",  // Travel Bag
        "HDPH",  // Headphones
        "CBL1",  // USB-C Cable
        "KEYM",  // Mechanical Keyboard
        "MSEW",  // Wireless Mouse
        "CHG1",  // Phone Charger
        "FANX"   // Portable Fan
    ];

    public IEnumerable<string> GetProducts()
    {
        return _products;
    }

    public string GetProduct(string productCode)
    {
        return _products.FirstOrDefault(p => p.Equals(productCode, StringComparison.InvariantCultureIgnoreCase));
    }
}
using System.Text.Json;
using ApiVersioning.Models;

namespace WebApplication3.Helpers
{
    public static class FileHelper
    {
        private static readonly string filePath = "Data/products.json";

        public static List<Product> LoadProducts()
        {
            if (!File.Exists(filePath))
                return new List<Product>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        public static void SaveProducts(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}

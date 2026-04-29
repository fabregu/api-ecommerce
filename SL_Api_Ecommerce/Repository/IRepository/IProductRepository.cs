using SL_Api_Ecommerce.Models;

namespace SL_Api_Ecommerce.Repository.IRepository
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        ICollection<Product> GetProductsforCategory(int categoryId);
        ICollection<Product> SearchProduct(string name);
        bool BuyProduct(string name, int quantity);
        Product? GetProduct(int productId);
        bool ProductExists(int id);
        bool ProductExists(string name);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
    }
}
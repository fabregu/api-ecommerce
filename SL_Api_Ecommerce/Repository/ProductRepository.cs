using Microsoft.EntityFrameworkCore;
using SL_Api_Ecommerce.Data;
using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Repository.IRepository;

namespace SL_Api_Ecommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool BuyProduct(string name, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
            {
                return false;
            }
            var product = _db.Products.FirstOrDefault(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
            if (product == null || product.Stock < quantity)
            {
                return false;
            }
            product.Stock -= quantity;
            _db.Products.Update(product);
            return Save();
        }

        public bool CreateProduct(Product product)
        {
            if(product == null)
            {
                return false;
            }
            product.CreationDate = DateTime.Now;
            product.UpdateDate = DateTime.Now;

            _db.Products.Add(product);
            return Save();
        }

        public Product? GetProduct(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            return _db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
        }

        public ICollection<Product> GetProducts()
        {
            return _db.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        }

        public ICollection<Product> GetProductsforCategory(int categoryId)
        {
            if(categoryId <= 0)
            {
                return new List<Product>();
            }
            return _db.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name).ToList();
        }

        public bool DeleteProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }
            _db.Products.Remove(product);
            return Save();
        }

        public bool ProductExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            return _db.Products.Any(p => p.ProductId == id);
        }

        public bool ProductExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            return _db.Products.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool UpdateProduct(Product product)
        {
            if(product == null)
            {
                return false;
            }
            product.UpdateDate = DateTime.Now;
            _db.Products.Update(product);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public ICollection<Product> SearchProducts(string searchTerm)
        {
            IQueryable<Product> query = _db.Products;

            var searchTermLowered = searchTerm.ToLower().Trim();
            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Include(p => p.Category).Where(
                    p => p.Name.ToLower().Trim().Contains(searchTermLowered) ||
                    p.Description.ToLower().Trim().Contains(searchTerm.ToLower().Trim()));
            }
            return query.OrderBy(p => p.Name).ToList();
        }
    }
}
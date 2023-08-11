using TalabatBLL.Entities;
using TalabatBLL.Interfaces;
using TalabatDAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TalabatDAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
            => await _context.ProductBrands.ToListAsync();

        public async Task<Product> GetproductByIdAsync(int? id)
            => await _context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand).FirstOrDefaultAsync(x => x.Id == id);


        public async Task<IReadOnlyList<Product>> GetProductsAsync()
            => await _context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand).ToListAsync();

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
            => await _context.ProductTypes.ToListAsync();


    }
}

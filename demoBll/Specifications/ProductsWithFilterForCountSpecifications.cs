using TalabatBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatBLL.Specifications
{
    public class ProductsWithFilterForCountSpecifications : BaseSpecification<Product>
    {
        public ProductsWithFilterForCountSpecifications(ProductSpecificationParameters productParameters)
            : base(x =>
                (string.IsNullOrEmpty(productParameters.Search) || x.Name.ToLower().Contains(productParameters.Search)) &&
                (!productParameters.BrandId.HasValue || x.ProductBrandId == productParameters.BrandId) &&
                (!productParameters.TypeId.HasValue || x.ProductTypeId == productParameters.TypeId)
            )
        {

        }
    }
}

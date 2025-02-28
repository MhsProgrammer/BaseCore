using BaseCore.Domain.Entities;


namespace BaseCore.Domain.Specifications.ProductSpec
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams productSpecParams) : base()
        {
            ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);
        }

        
    } 
}

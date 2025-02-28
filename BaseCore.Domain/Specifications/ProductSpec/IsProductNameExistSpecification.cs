using BaseCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Domain.Specifications.ProductSpec
{
    public class IsProductNameExistSpecification : BaseSpecification<Product>
    {
        public IsProductNameExistSpecification(string productName) : 
            base(x => x.ProductName == productName)
        {
            
        }
    }
}

using System.Collections.Generic;

namespace Models
{
    public class ProductType : BaseEntity
    {
        public ProductType()
        {
            Products = new List<Product>();
        }

        public string Title { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
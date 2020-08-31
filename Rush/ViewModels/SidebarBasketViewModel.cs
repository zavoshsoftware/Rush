using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class SidebarBasketViewModel
    {
        public List<ProductInSidebar> Products { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public string DiscountAmount { get; set; }

    }

    public class ProductInSidebar
    {

        public string Id { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }

        public string RowAmount
        {
            get
            {
                DatabaseContext db = new DatabaseContext();

                Guid productId = new Guid(Id);

                Product product = db.Products.Find(productId);

                if (product.ProductType.Name == "reportage")
                {
                    Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == productId);

                    if (reportage != null)
                    {
                        if (reportage.IsInPromotion && reportage.DiscountAmount != null)
                            return (reportage.DiscountAmount.Value * Quantity).ToString("n0");

                        return (reportage.Price * Quantity).ToString("n0");
                    }
                }

                if (product.ProductType.Name == "backlink")
                {
                    BackLinkDetail backLinkDetail = db.BackLinkDetails.FirstOrDefault(c => c.ProductId == productId);

                    if (backLinkDetail != null)
                    {
                      
                        return (backLinkDetail.Amount * Quantity).ToString("n0");
                    }
                }

                if (product.ProductType.Name == "package")
                {
                    ReportageGroup reportageGroup = db.ReportageGroups.FirstOrDefault(c => c.ProductId == productId);

                    if (reportageGroup != null)
                    {
                        return (reportageGroup.Value * Quantity).Value.ToString("n0");
                    }
                }

                return String.Empty;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        public List<ProductInCart> Products { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public string DiscountAmount { get; set; }

    }

    public class ProductInCart
    {
        public string Id { get; set; }
        private DatabaseContext db = new DatabaseContext();
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string Amount
        {
            get
            {
                Guid productId = new Guid(Id);

                if (Product.ProductType.Name == "reportage")
                {
                    Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == Product.Id);

                    if (reportage.IsInPromotion && reportage.DiscountAmount != null)
                        return (reportage.DiscountAmount.Value).ToString("n0") + " تومان";

                    return (reportage.Price).ToString("n0") + " تومان";
                }

               else if (Product.ProductType.Name == "backlink")
                {
                    BackLinkDetail backLinkDetail = db.BackLinkDetails.FirstOrDefault(c => c.ProductId == Product.Id);

                  

                    return (backLinkDetail.Amount).ToString("n0") + " تومان";
                }

                if (Product.ProductType.Name == "package")
                {
                    ReportageGroup reportageGroup = db.ReportageGroups.FirstOrDefault(c => c.ProductId == productId);

                    if (reportageGroup != null)
                    {
                        if (reportageGroup.Value != null)
                            return (reportageGroup.Value).Value.ToString("n0");
                    }
                }
                return String.Empty;

            }
        }
        public string RowAmount
        {
            get
            {
                Guid productId = new Guid(Id);


                if (Product.ProductType.Name == "reportage")
                {
                    Reportage reportage = db.Reportages.FirstOrDefault(c => c.ProductId == Product.Id);

                    if (reportage.IsInPromotion && reportage.DiscountAmount != null)
                        return (reportage.DiscountAmount.Value * Quantity).ToString("n0") + " تومان";

                    return (reportage.Price * Quantity).ToString("n0") + " تومان";

                }
                if (Product.ProductType.Name == "package")
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
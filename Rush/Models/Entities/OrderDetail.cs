using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderDetail : BaseEntity
    {
        public OrderDetail()
        {
            OrderDetailInformations = new List<OrderDetailInformation>();
        }

        [Display(Name = "OrderId")]
        public Guid OrderId { get; set; }

        [Display(Name = "محصول")]
        public Guid ProductId { get; set; }

        [Display(Name = "تعداد")]
        public int Quantity { get; set; }

        [Display(Name = "فی")]
        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        [Display(Name = "قیمت")]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        [Display(Name = "FileUrl")]
        public string FileUrl { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public Guid OrderDetailStatusId { get; set; }
        public virtual OrderDetailStatus OrderDetailStatus { get; set; }

        [Display(Name = "لینک انتشار")]
        public string PublishLink { get; set; }

        [Display(Name = "پیامک انتشار ارسال شد؟")]
        public bool IsSendPublishSms { get; set; }

        public virtual ICollection<OrderDetailInformation> OrderDetailInformations { get; set; }
        internal class Configuration : EntityTypeConfiguration<OrderDetail>
        {
            public Configuration()
            {
                HasRequired(p => p.Order)
                    .WithMany(j => j.OrderDetails)
                    .HasForeignKey(p => p.OrderId);

                HasRequired(p => p.Product)
                    .WithMany(j => j.OrderDetails)
                    .HasForeignKey(p => p.ProductId);

                HasRequired(p => p.OrderDetailStatus)
                    .WithMany(j => j.OrderDetails)
                    .HasForeignKey(p => p.OrderDetailStatusId);
            }
        }

        [NotMapped]
        [Display(Name = "فی")]
        public string PriceStr
        {
            get { return Price.ToString("N0"); }
        }

        [NotMapped]
        [Display(Name = "قیمت")]
        public string AmountStr
        {
            get { return Amount.ToString("N0"); }
        }
    }
}

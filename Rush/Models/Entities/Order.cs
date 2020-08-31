using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc.Html;

namespace Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            ZarinpallAuthorities = new List<ZarinpallAuthority>();
        }

        [Display(Name = "کد سفارش")]
        [Required]
        public int Code { get; set; }

        [Display(Name = "کاربر")]
        public Guid? UserId { get; set; }


        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "قیمت نهایی")]
        [Column(TypeName = "Money")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "وضعیت")]
        [Required]
        public Guid OrderStatusId { get; set; }

        [Display(Name = "کد رهگیری")]
        public string RefId { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "پرداخت شده؟")]
        public bool IsPaid { get; set; }
        public Guid? DiscountCodeId { get; set; }

        public virtual DiscountCode DiscountCode { get; set; }
 

        [Display(Name = "هزینه جمع فاکتور")]
        public decimal? SubTotal { get; set; }


        [Display(Name = "مبلغ تخفیف")]
        public decimal? DiscountAmount { get; set; }

        public virtual List<ZarinpallAuthority> ZarinpallAuthorities { get; set; }
        internal class Configuration : EntityTypeConfiguration<Order>
        {
            public Configuration()
            {
                HasOptional(p => p.User)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.UserId);

                HasRequired(p => p.OrderStatus)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.OrderStatusId);

              

                HasRequired(p => p.DiscountCode)
                    .WithMany(j => j.Orders)
                    .HasForeignKey(p => p.DiscountCodeId);
            }
        }
        [Display(Name = "نام کاربر")]
        public string DeliverFullName { get; set; }
        [Display(Name = "شماره کاربر")]

        public string DeliverCellNumber { get; set; }
        [Display(Name = "تاریخ پرداخت")]

        public DateTime? PaymentDate { get; set; }


        [NotMapped]
        [Display(Name = "قیمت نهایی")]
        public string TotalAmountStr
        {
            get { return TotalAmount.ToString("N0"); }
        }

        [NotMapped]
        [Display(Name = "هزینه جمع فاکتور")]
        public string SubTotalStr
        {
            get
            {
                if(SubTotal!=null)
                return SubTotal.Value.ToString("N0");
                return string.Empty;

            }
        }

        [NotMapped]
        [Display(Name = "مبلغ تخفیف")]
        public string DiscountAmountStr
        {
            get
            {
                if (DiscountAmount != null)
                    return DiscountAmount.Value.ToString("N0");
                return string.Empty;
            }
        }

        public string OrderType { get; set; }
    }
}

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
    public class OrderDetailInformation : BaseEntity
    {
        [Display(Name = "OrderDetailId")]
        public Guid OrderDetailId { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
        
        [Display(Name = "FileUrl")]
        public string FileUrl { get; set; }

        [Display(Name = "وضعیت")]
        public Guid? OrderDetailStatusId { get; set; }

        public virtual OrderDetailStatus OrderDetailStatus { get; set; }

        public Guid? ProductId { get; set; }
        public virtual Product Product { get; set; }
        
        [Display(Name = "لینک انتشار")]
        public string PublishLink { get; set; }


        [Display(Name="کلمه کلیدی")]
        public string BacklinkKeyword { get; set; }
        [Display(Name="آدرس لینک")]
        public string BacklinkUrl { get; set; }

        [Display(Name = "زمان ثبت بک لینک")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "زمان انقضا بک لینک")]
        public DateTime? FinishDate { get; set; }


        [Display(Name = "پیامک انتشار ارسال شد؟")]
        public bool IsSendPublishSms { get; set; }
        internal class Configuration : EntityTypeConfiguration<OrderDetailInformation>
        {
            public Configuration()
            {
                HasRequired(p => p.OrderDetail)
                    .WithMany(j => j.OrderDetailInformations)
                    .HasForeignKey(p => p.OrderDetailId);

                HasOptional(p => p.OrderDetailStatus)
                    .WithMany(j => j.OrderDetailInformations)
                    .HasForeignKey(p => p.OrderDetailStatusId);

                HasOptional(p => p.Product)
                    .WithMany(j => j.OrderDetailInformations)
                    .HasForeignKey(p => p.ProductId);
            }
        }
 
    }
}

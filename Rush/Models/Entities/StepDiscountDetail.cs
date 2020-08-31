using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class StepDiscountDetail:BaseEntity
    {
        public Guid StepDiscountId { get; set; }
        [Display(Name="پله هدف")]
        public decimal TargetValue { get; set; }
        [Display(Name="درصد تخفیف")]
        public decimal DiscountPercent { get; set; }
        public virtual StepDiscount StepDiscount { get; set; }

        internal class Configuration : EntityTypeConfiguration<StepDiscountDetail>
        {
            public Configuration()
            {
                HasRequired(p => p.StepDiscount)
                    .WithMany(j => j.StepDiscountDetails)
                    .HasForeignKey(p => p.StepDiscountId);
            }
        }
    }
}
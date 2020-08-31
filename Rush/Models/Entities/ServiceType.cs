using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class ServiceType : BaseEntity
    {
        public ServiceType()
        {
            ServiceForms = new List<ServiceForm>();
        }
        [Display(Name = "نوع خدمت")]
        public string Title { get; set; }
        public virtual ICollection<ServiceForm> ServiceForms { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ServiceGroupDetialWebDesingViewModel : BaseViewModel
    {
        public ServiceGroup ServiceGroup { get; set; }
        public ServiceForm ServiceForm { get; set; }
        public List<AskedQuestion> Questions { get; set; }
        public Models.Text TxtHeader { get; set; }
        public Models.Text TxtSection2 { get; set; }
        public Models.Text TxtWhyNeedSite { get; set; }
        public Models.Text TxtSitePrice { get; set; }
        public Models.Text TxtSpecialWebDesign { get; set; }
        public Models.Text TxtEshopSite { get; set; }
        public Models.Text TxtEnterpriseSite { get; set; }
        public Models.Text TxtWordpreeSite { get; set; }
        public List<Portfolio> Portfolios { get; set; }
    }
}
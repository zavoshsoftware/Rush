using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ServiceGroupDetialViewModel:BaseViewModel
    {
        public ServiceGroup ServiceGroup { get; set; }
        public ServiceForm ServiceForm { get; set; }
        public List<AskedQuestion> Questions { get; set; }
        public List<Portfolio> Portfolios { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ServiceDetailViewModel:BaseViewModel
    {
        public Service Service { get; set; }
        public List<AskedQuestion> Questions { get; set; }
    }
}
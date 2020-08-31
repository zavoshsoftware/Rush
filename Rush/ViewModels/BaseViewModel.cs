using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BaseViewModel
    {
        public List<Menu> Menu { get; set; }
        public string FooterLink { get; set; }
        public string Rate { get; set; }
        public string Username { get; set; }
    }
    public class Menu
    {
        public ServiceGroup ServiceGroup { get; set; }
        public List<Service> Services { get; set; }
         public bool HasChild { get; set; }
    }
}
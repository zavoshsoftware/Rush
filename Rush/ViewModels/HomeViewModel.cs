using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class HomeViewModel:BaseViewModel
    {
        public List<Blog> Blogs { get; set; }
        public string SliderBoldText { get; set; }
        public string SliderText { get; set; }
        public List<Text> UnderSliderText { get; set; }
        public List<Text> Chooseus { get; set; }
        public TextType WhyRush { get; set; }
        public string MiddleText { get; set; }
        public Text UnderSliderMainText { get; set; }
        public List<Team> TeamMembers { get; set; }
        public List<Customer> Customers { get; set; }
        public Contact_Index ContactInfo { get; set; }

    }
    public class Contact_Index
    {
        public TextType MainText { get; set; }
        public Text Address { get; set; }
        public Text Phone { get; set; }
        public Text Email { get; set; }

    }
}
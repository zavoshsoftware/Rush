using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BackLinkViewModel:BaseViewModel
    {
        //public Text Text { get; set; }
        public List<BackLinkItem> BackLinks { get; set; }
        public string Body { get; set; }
        public string BottomText { get; set; }
        public List<AskedQuestion> Questions { get; set; }

    }

    public class BackLinkItem
    {
        public BackLink BackLink { get; set; }
        public List<KeyValueViewModel> BackLinkDetails { get; set; }
    }
}
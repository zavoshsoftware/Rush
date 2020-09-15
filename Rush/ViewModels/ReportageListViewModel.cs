using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ReportageListViewModel:BaseViewModel
    {
        public Text Text { get; set; }

        //public List<Reportage> Reportages { get; set; }
        public List<ReportageByGroup> ReportageByGroups { get; set; }
        public List<ReportageGroup> ReportageGroups { get; set; }
        public List<AskedQuestion> Questions { get; set; }
        public string BottomText { get; set; }
        public List<Reportage> SpecialReportages { get; set; }
    }

    public class ReportageByGroup
    {
        public Guid ReportageGroupId { get; set; }
        public string ReportageGroupTitle { get; set; }
        public List<Reportage> Reportages { get; set; }
        public decimal? Value { get; set; }
        public decimal? Price { get; set; }
    }
}
using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public DateTime DateCreate { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public SxSiteTest.SiteTestType Type { get; set; }
    }
}

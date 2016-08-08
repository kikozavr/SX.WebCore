using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTest
    {
        public SxVMSiteTest()
        {
            Questions = new SxVMSiteTestQuestion[0];
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public DateTime DateCreate { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public SxSiteTest.SiteTestType Type { get; set; }
        public SxVMSiteTestQuestion[] Questions { get; set; }
        public string Rules { get; set; }
        public int ViewsCount { get; set; }
    }
}

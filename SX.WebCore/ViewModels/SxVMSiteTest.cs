using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreate { get; set; }
        public string Description { get; set; }
        public SxSiteTest.SiteTestType TestType { get; set; }
    }
}

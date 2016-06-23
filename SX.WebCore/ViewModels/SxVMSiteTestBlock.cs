using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestBlock
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public SxVMSiteTest Test { get; set; }
        public int TestId { get; set; }

        public string TestTitle
        {
            get
            {
                return Test?.Title;
            }
            set { }
        }

        public DateTime DateCreate { get; set; }

        public string Description { get; set; }
    }
}

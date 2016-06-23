using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestQuestion
    {
        public int Id { get; set; }

        public SxVMSiteTest Test { get; set; }
        public string TestTitle
        {
            get
            {
                return Block?.Test?.Title;
            }
            set { }
        }

        public SxVMSiteTestBlock Block { get; set; }
        public string BlockTitle
        {
            get
            {
                return Block?.Title;
            }
            set { }
        }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public DateTime DateCreate { get; set; }
    }
}

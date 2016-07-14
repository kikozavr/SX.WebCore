using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestQuestion
    {
        public int Id { get; set; }

        public SxVMSiteTest Test { get; set; }

        public string Text { get; set; }

        public DateTime DateCreate { get; set; }
    }
}

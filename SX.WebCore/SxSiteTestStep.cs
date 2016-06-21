using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [NotMapped]
    public sealed class SxSiteTestStep
    {
        public SxSiteTestQuestion Question { get; set; }
        public int Order { get; set; }
    }
}

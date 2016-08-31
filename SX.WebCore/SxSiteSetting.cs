using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_SETTING")]
    public sealed class SxSiteSetting : SxDbUpdatedModel<string>
    {
        public string Value { get; set; }

        public string Description { get; set; }
    }
}

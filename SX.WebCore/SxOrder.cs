using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore
{
    [NotMapped]
    public sealed class SxOrder
    {
        public string FieldName { get; set; }
        public SortDirection Direction { get; set; }
    }
}

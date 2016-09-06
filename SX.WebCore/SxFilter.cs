using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore
{
    [NotMapped]
    public class SxFilter
    {
        public SxFilter()
        {
            PagerInfo = new SxPagerInfo(1, 10);
        }

        public SxFilter(int page, int pageSize)
        {
            PagerInfo = new SxPagerInfo(page,pageSize);
        }
        public SxPagerInfo PagerInfo { get; set; }
        public SxMaterialTag Tag { get; set; }
        public string CategoryId { get; set; }
        public int? MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public dynamic WhereExpressionObject { get; set; }
        public object[] AddintionalInfo { get; set; }
        public SxOrder Order { get; set; }
        public bool? OnlyShow { get; set; }
    }
}

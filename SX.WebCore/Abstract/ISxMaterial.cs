using static SX.WebCore.Enums;

namespace SX.WebCore.Abstract
{
    public interface ISxMaterial : ISxHasHtml, ISxHasFrontPicture, IHasViewsCount
    {
        ModelCoreType ModelCoreType { get; set; }
    }
}

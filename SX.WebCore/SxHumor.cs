using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_HUMOR")]
    public class SxHumor: SxMaterial
    {
        public SxHumor()
        {
            ModelCoreType = Enums.ModelCoreType.Humor;
        }
    }
}

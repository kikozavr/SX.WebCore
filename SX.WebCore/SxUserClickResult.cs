using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [NotMapped]
    public sealed class SxUserClickResult
    {
        public Enums.UserClickResult Result { get; set; }
        public int Count { get; set; }
    }
}

using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMRating
    {
        public string UserId { get; set; }
        public SxVMAppUser User { get; set; }

        public int Value { get; set; }

        public string SessionId { get; set; }

        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
    }
}

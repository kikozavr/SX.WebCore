using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public abstract class SxVMMaterial
    {
        public SxVMMaterial()
        {
            Videos = new SxVideo[0];
        }

        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public SxVideo[] Videos { get; set; }
    }
}

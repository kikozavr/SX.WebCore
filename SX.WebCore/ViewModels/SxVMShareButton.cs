namespace SX.WebCore.ViewModels
{
    public sealed class SxVMShareButton
    {
        public int Id { get; set; }

        public SxNet Net { get; set; }
        public int NetId { get; set; }

        public string NetName { get; set; }

        public bool Show { get; set; }

        public bool ShowCounter { get; set; }
    }
}

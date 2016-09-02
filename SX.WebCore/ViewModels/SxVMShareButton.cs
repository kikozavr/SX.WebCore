using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMShareButton
    {
        public int Id { get; set; }

        public SxVMNet Net { get; set; }
        public int NetId { get; set; }

        public string NetName { get; set; }

        [Display(Name = "Показывать")]
        public bool Show { get; set; }

        [Display(Name = "Показывать счетчик")]
        public bool ShowCounter { get; set; }
    }
}

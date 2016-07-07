using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditShareButton
    {
        public int Id { get; set; }

        public SxNet Net { get; set; }
        public int NetId { get; set; }

        [Display(Name ="Показывать")]
        public bool Show { get; set; }

        [Display(Name = "Показывать счетчик")]
        public bool ShowCounter { get; set; }
    }
}

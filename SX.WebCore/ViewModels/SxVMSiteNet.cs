using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteNet
    {
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Url(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ValidUrlField")]
        [Display(Name ="Адрес")]
        public string Url { get; set; }

        [Display(Name = "Показывать")]
        public bool Show { get; set; }

        public int NetId { get; set; }
        public SxVMNet Net { get; set; }

        public string NetName
        {
            get
            {
                return Net?.Name;
            }
            set { }
        }
    }
}

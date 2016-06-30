using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestSubject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public SxVMSiteTest Test { get; set; }

        public SxVMPicture Picture { get; set; }
        public Guid? PictureId { get; set; }
    }
}

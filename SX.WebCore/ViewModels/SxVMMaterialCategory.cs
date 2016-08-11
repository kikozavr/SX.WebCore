using SX.WebCore.Abstract;
using System;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public class SxVMMaterialCategory : IHierarchy<SxVMMaterialCategory>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public ModelCoreType ModelCoreType { get; set; }

        public virtual SxVMMaterialCategory ParentCategory { get; set; }
        public string ParentCategoryId { get; set; }

        public virtual SxVMPicture FrontPicture { get; set; }
        public Guid? FrontPictureId { get; set; }

        public int Level { get; set; }

        public SxVMMaterialCategory[] ChildCategories { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditManual
    {
        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public string UserId { get; set; }

        public SxVMEditMaterialCategory Category { get; set; }
        [Display(Name = "Категория"), UIHint("MaterialCategoryLookupGrid"), AdditionalMetadata("mct", ModelCoreType.Manual)]
        public string CategoryId { get; set; }

        [Display(Name = "Название материала"), MaxLength(255), Required]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }
    }
}

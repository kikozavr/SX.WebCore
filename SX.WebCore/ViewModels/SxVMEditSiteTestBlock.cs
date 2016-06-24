using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditSiteTestBlock
    {
        public int Id { get; set; }

        [Required, Display(Name = "Тест"), UIHint("SiteTestBlocksLookupGrid")]
        public int TestId { get; set; }

        [Required, MaxLength(100), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required, MaxLength(1000), Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

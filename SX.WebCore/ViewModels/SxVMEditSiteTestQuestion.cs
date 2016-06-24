using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditSiteTestQuestion
    {
        public int Id { get; set; }

        public SxVMSiteTest Test { get; set; }

        [Required, Display(Name = "Тест"), UIHint("SiteTestsLookupGrid")]
        public int TestId { get; set; }

        public SxVMSiteTestBlock Block { get; set; }

        [Required, Display(Name = "Блок"), UIHint("SiteTestBlocksLookupGrid")]
        public int BlockId { get; set; }

        [Required, MaxLength(400), DataType(DataType.MultilineText), Display(Name = "Вопрос")]
        public string Text { get; set; }

        [Required, Display(Name = "Пометить правильным")]
        public bool IsCorrect { get; set; }
    }
}

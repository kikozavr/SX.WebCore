using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditSiteTestQuestion
    {
        public int Id { get; set; }

        public SxVMSiteTest Test { get; set; }

        [Required, Display(Name = "Тест"), UIHint("SiteTestsLookupGrid")]
        public int TestId { get; set; }

        [Required, MaxLength(400), Display(Name = "Вопрос")]
        public string Text { get; set; }
    }
}

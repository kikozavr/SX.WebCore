using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_SEO_INFO")]
    public class SxSeoInfo : SxDbUpdatedModel<int>
    {
        [MaxLength(255), Required]
        public string SeoTitle { get; set; }

        [MaxLength(1000)]
        public string SeoDescription { get; set; }

        public virtual ICollection<SxSeoKeyword> Keywords { get; set; }

        [MaxLength(255), Index]
        public string RawUrl { get; set; }

        [MaxLength(80), Index]
        public string H1 { get; set; }

        [MaxLength(20)]
        public string H1CssClass { get; set; }

        public virtual SxMaterial Material { get; set; }
        public int? MaterialId { get; set; }
        public ModelCoreType? ModelCoreType { get; set; }

        private static int _seoTitleMinWordsCount = 7;
        private static int _seoDescMinCharsCount = 150;
        private static int _seoDescMaxCharsCount = 700;
        private static int _seoH1MinWordsCount = 2;
        public void Check(System.Web.Mvc.ModelStateDictionary modelState)
        {

            //seo title
            var seoTitleWordsCount = SeoTitle == null ? 0 : SeoTitle.Split(' ').Length;
            if (seoTitleWordsCount < _seoTitleMinWordsCount)
                modelState.AddModelError("SeoTitle", string.Format("Значение тега Title не может быть меньше {0} слов", _seoTitleMinWordsCount));

            //seo desc
            var seoDescCharsCount = SeoDescription == null ? 0 : SeoDescription.Length;
            if (seoDescCharsCount < _seoDescMinCharsCount)
                modelState.AddModelError("SeoDescription", string.Format("Значение тега SeoDescription не может быть меньше {0} знаков", _seoDescMinCharsCount));
            if (seoDescCharsCount > _seoDescMaxCharsCount)
                modelState.AddModelError("SeoDescription", string.Format("Значение тега SeoDescription не может быть больше {0} знаков", _seoDescMaxCharsCount));

            //seo H1
            var seoH1WordsCount = H1 == null ? 0 : H1.Split(' ').Length;
            if (seoH1WordsCount < _seoH1MinWordsCount)
                modelState.AddModelError("H1", string.Format("Значение тега H1 не может быть меньше {0} слов", _seoH1MinWordsCount));
        }
    }
}

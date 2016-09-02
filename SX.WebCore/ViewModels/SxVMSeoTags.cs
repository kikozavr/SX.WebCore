using SX.WebCore.Attrubutes;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSeoTags
    {
        public SxVMSeoTags()
        {
            Keywords = new SxVMSeoKeyword[0];
        }

        public int Id { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Адрес")]
        public string RawUrl { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Заголовок страницы"), MaxWordsCount(15), MinWordsCount(7)]
        public string SeoTitle { get; set; }

        [MaxLength(700), MinLength(150) Display(Name = "Описание страницы"), DataType(DataType.MultilineText)]
        public string SeoDescription { get; set; }

        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Тег h1"), MaxWordsCount(6), MinWordsCount(2)]
        public string H1 { get; set; }

        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Css стиль тега h1")]
        public string H1CssClass { get; set; }

        public SxVMSeoKeyword[] Keywords { get; set; }

        public string KeywordsString
        {
            get
            {
                if (Keywords==null || !Keywords.Any()) return null;

                var sb = new StringBuilder();
                for (int i = 0; i < Keywords.Length; i++)
                {
                    sb.AppendFormat(", {0}", Keywords[i].Value);
                    sb.Remove(0, 2);
                }
                return sb.ToString();
            }
        }

        public int? MaterialId { get; set; }
        public ModelCoreType? ModelCoreType { get; set; }
    }
}

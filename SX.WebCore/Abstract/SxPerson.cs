using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.Abstract
{
    public abstract class SxPerson : SxDbModel<string>
    {
        public virtual SxAppUser User { get; set; }

        [Required, MaxLength(128)]
        public override string Id { get; set; }
    }
}

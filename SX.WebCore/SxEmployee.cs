using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_EMPLOYEE")]
    public sealed class SxEmployee : SxPerson
    {
        [MaxLength(50)]
        public string Surname { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Patronymic { get; set; }

        public string Description { get; set; }
    }
}

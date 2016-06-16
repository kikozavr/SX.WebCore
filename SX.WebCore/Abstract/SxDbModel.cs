using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore.Abstract
{
    public abstract class SxDbModel<TKey>
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreate { get; set; }
    }
}

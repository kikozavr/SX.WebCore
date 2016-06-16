using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.Abstract
{
    public abstract class SxDbUpdatedModel<TKey> : SxDbModel<TKey>
    {
        [DataType(DataType.DateTime)]
        public DateTime DateUpdate { get; set; }
    }
}

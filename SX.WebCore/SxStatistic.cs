using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_STATISTIC")]
    public class SxStatistic : SxDbModel<Guid>
    {
        public SxStatisticType Type { get; set; }

        public virtual ICollection<SxStatisticUserLogin> UserLogins { get; set; }

        public enum SxStatisticType : byte
        {
            Unknown=0,
            UserLogin=1
        }
    }
}

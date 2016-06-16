using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_STAT_LOGIN")]
    public class SxStatisticUserLogin
    {
        public virtual SxStatistic Statistic { get; set; }
        public Guid StatisticId { get; set; }

        public virtual SxAppUser User { get; set; }
        [Required, MaxLength(128)]
        public string UserId { get; set; }
    }
}

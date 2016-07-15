using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [NotMapped]
    public sealed class SxDateStatistic
    {
        public int Count { get; set; }
        public DateTime DateCreate { get; set; }
    }
}

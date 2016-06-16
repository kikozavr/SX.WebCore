using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    [Table("D_SITE_SETTING")]
    public sealed class SxSiteSetting : SxDbUpdatedModel<string>
    {
        //[Column("VALUE"), Required, MaxLength(255)]
        public string Value { get; set; }

        //[Column("DESCRIPTION"), MaxLength(100)]
        public string Description { get; set; }

        public override string ToString()
        {
            return this.Value;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SX.WebCore
{
    /// <summary>
    /// http://www.sitemaps.org/ru/protocol.html
    /// </summary>
    [NotMapped]
    public sealed class SxSiteMapUrl
    {
        public SxSiteMapUrl(string loc)
        {
            Loc = loc;
        }
        public string Loc
        {
            get;
            private set;
        }
        public DateTime LasMod { get; set; }
        public Changefreqs Changefreq { get; set; }
        public decimal Priority { get; set; }
        

        public enum Changefreqs : byte
        {
            Unknown = 0,
            Always = 1,
            Hourly = 2,
            Daily = 3,
            Weekly = 4,
            Monthly = 5,
            Yearly = 6,
            Never = 7
        }
    }
}

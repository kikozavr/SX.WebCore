using System.Collections.Generic;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Linq;

namespace SX.WebCore.Providers
{
    public static class SxQueryProvider
    {
        public static string GetSelectString(string[] columns = null)
        {
            var sb = new StringBuilder();
            if (columns == null)
            {
                sb.Append(",* ");
            }
            else
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    sb.Append("," + column);
                }
            }
            var s = sb.ToString();
            return "SELECT " + s.Substring(1);
        }

        public static string GetOrderString(string dc, SortDirection dsd, IDictionary<string, SortDirection> orders = null)
        {
            var orderCount = orders!=null? orders.Where(x => x.Value != SortDirection.Unknown).Count():0;
            var sb = new StringBuilder();
            if (orders == null || orderCount==0)
            {
                sb.AppendFormat(",{0} {1}", dc, dsd.ToString().ToUpper());
            }
            else
            {
                foreach (var order in orders.Where(x=>x.Value!=SortDirection.Unknown))
                {
                    sb.AppendFormat(",{0} {1}", order.Key, order.Value.ToString().ToUpper());
                }
            }

            var s = sb.ToString();

            return "ORDER BY " + s.Substring(1);
        }
    }
}

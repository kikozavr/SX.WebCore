using System.Collections.Generic;
using System.Text;

namespace SX.WebCore.Providers
{
    public static class SxQueryProvider
    {
        public static string GetSelectString(string[] columns = null, bool isDistinct=false)
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
            return "SELECT " + (isDistinct ?"DISTINCT ":null)+ s.Substring(1);
        }

        public static string GetOrderString(SxOrder defaultOrder, SxOrder order=null, Dictionary<string, string> replaceList=null)
        {
            var sb = new StringBuilder();
            if (order==null || order.FieldName==null)
                sb.AppendFormat(",{0} {1}", replaceList != null && replaceList.ContainsKey(defaultOrder.FieldName) ? replaceList[defaultOrder.FieldName] : defaultOrder.FieldName, defaultOrder.Direction.ToString().ToUpper());
            else
                sb.AppendFormat(",{0} {1}", replaceList!=null && replaceList.ContainsKey(order.FieldName) ? replaceList[order.FieldName]: order.FieldName, order.Direction.ToString().ToUpper());

            sb.Remove(0, 1);
            return "ORDER BY "+sb.ToString();
        }
    }
}

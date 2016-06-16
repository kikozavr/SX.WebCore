using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Providers
{
    public class SxErrorProvider
    {
        private static string _dir;
        private static object _locko = new object();
        public SxErrorProvider(string dir)
        {
            _dir = dir;
        }

        private static void WriteMessage(string message)
        {
            lock(_locko)
            {
                Task.Run(() =>
                {
                    var date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!Directory.Exists(_dir))
                        Directory.CreateDirectory(_dir);

                    var filePath = Path.Combine(_dir, date + ".log");

                    var sb = new StringBuilder();
                    sb.AppendLine("Date: " + DateTime.Now.ToString());
                    sb.AppendLine(message);

                    File.AppendAllText(filePath, sb.ToString());
                });
            }
        }

        public virtual void WriteMessage(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("StackTrace: " + ex.StackTrace);
            WriteMessage(sb.ToString());
        }
    }
}

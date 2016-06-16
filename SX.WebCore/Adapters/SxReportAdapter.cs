using OfficeOpenXml;
using System;
using System.Data;
using System.Web;

namespace SX.WebCore.Adapters
{
    public abstract class SxReportAdapter : IDisposable
    {
        private bool disposed = false;

        public void Dispose()
        {
            cleanUp(true);
            GC.SuppressFinalize(this);
        }

        private void cleanUp(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
            }

            disposed = true;
        }

        ~SxReportAdapter()
        {
            cleanUp(false);
        }

        public abstract DataTable DataTable { get; }

        public void SendReport(HttpResponseBase response, string reportName)
        {
            if (response == null || DataTable == null || DataTable.Rows.Count == 0 || string.IsNullOrEmpty(reportName)) return;

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Demo");
                ws.Cells["A1"].LoadFromDataTable(DataTable, true);
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.AddHeader("content-disposition", "attachment;  filename=" + reportName + ".xlsx");
                response.BinaryWrite(pck.GetAsByteArray());
            }
        }
    }
}

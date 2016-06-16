using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Web.Mvc;

namespace SX.WebCore.Controllers
{
    public abstract class SxLogsController : Controller
    {
        [HttpGet]
        public virtual FileResult GetLog(string siteName=null)
        {
            var dir = Server.MapPath("~/Logs");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var zipname = string.Format("{0}-logs-{1}.zip", siteName ?? "site", DateTime.Now.ToString("yyyy-MM-dd"));

            var ms = new MemoryStream();
            var zipStream = new ZipOutputStream(ms);
            zipStream.SetLevel(3);

            foreach (string file in Directory.GetFiles(dir))
            {
                FileInfo fi = new FileInfo(file);

                string entryName = ZipEntry.CleanName(fi.Name);
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime;
                newEntry.Size = fi.Length;
                zipStream.PutNextEntry(newEntry);

                byte[] buffer = new byte[4096];
                using (FileStream streamReader = System.IO.File.OpenRead(fi.FullName))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            zipStream.IsStreamOwner = false;
            zipStream.Close();

            ms.Position = 0;

            string file_type = "application/zip";
            return File(ms, file_type, zipname);
        }
    }
}

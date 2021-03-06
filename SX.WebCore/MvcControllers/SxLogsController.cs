﻿using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using SX.WebCore.MvcControllers.Abstract;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxLogsController : SxBaseController
    {
        [HttpGet]
        public virtual async Task<FileResult> GetLog()
        {
            return await Task.Run(() =>
            {
                var dir = Server.MapPath("~/logs");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

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
                var domain =Url.SeoFriendlyUrl(Request.Url.Authority);
                var zipname = string.Format("{0}-logs-{1}.zip", domain, DateTime.Now.ToString("yyyy-MM-dd"));
                return File(ms, file_type, zipname);
            });
        }
    }
}

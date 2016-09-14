using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMAnalizatorSession
    {
        public SxVMAnalizatorSession()
        {
            Urls = new SxVMAnalizatorUrl[0];
        }

        public DateTime DateCreate { get; set; }

        public int Id { get; set; }

        public SxVMAppUser User { get; set; }

        public int UrlsCount { get; set; }

        public SxVMAnalizatorUrl[] Urls { get; set; }
    }
}

using System;

namespace SX.WebCore.ViewModels
{
    public class SxVMAnalizatorUrl
    {
        public DateTime DateCreate { get; set; }

        public string Url { get; set; }

        public int StatusCode { get; set; } = 200;
    }
}

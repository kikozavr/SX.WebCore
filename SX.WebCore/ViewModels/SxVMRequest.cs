using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMRequest
    {
        public Guid Id { get; set; }

        public DateTime DateCreate { get; set; }

        public string SessionId { get; set; }

        public string UrlRef { get; set; }

        public string Browser { get; set; }

        public string ClientIP { get; set; }

        public string UserAgent { get; set; }

        public string RequestType { get; set; }

        public string RawUrl { get; set; }
    }
}
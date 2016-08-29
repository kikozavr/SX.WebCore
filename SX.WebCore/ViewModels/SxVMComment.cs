using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMComment
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Html { get; set; }
        public SxVMAppUser User { get; set; }
    }
}

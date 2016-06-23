using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMRobotsFile
    {
        [Display(Name = "Содержание файла"), Required, DataType(DataType.MultilineText)]
        public string FileContent { get; set; }
        public string OldFileContent { get; set; }
    }
}
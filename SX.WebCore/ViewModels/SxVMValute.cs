using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore.ViewModels
{
    [NotMapped]
    public sealed class SxVMValute
    {
        public string Id { get; set; }
        public Int16 NumCode { get; set; }
        public string CharCode { get; set; }
        public decimal Nominal { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}

using SX.WebCore.Attrubutes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEdit301Redirect
    {
        public Guid? Id { get; set; }

        [MaxLength(255), Required, Display(Name = "Старый адрес")]
        public string OldUrl { get; set; }

        [MaxLength(255), Required, Display(Name = "Новый адрес"), NotEqual("OldUrl")]
        public string NewUrl { get; set; }
    }
}

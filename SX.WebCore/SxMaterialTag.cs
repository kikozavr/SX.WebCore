﻿using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_MATERIAL_TAG")]
    public class SxMaterialTag : SxDbUpdatedModel<string>
    {
        [MaxLength(100), Required]
        public string Title { get; set; }

        public SxMaterial Material { get; set; }
        public int? MaterialId { get; set; }
        public Enums.ModelCoreType ModelCoreType { get; set; }
    }
}

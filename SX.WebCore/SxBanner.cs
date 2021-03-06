﻿using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_BANNER")]
    public class SxBanner : SxDbUpdatedModel<Guid>
    {
        [MaxLength(100)]
        public string Title { get; set; }

        public SxPicture Picture { get; set; }
        public Guid PictureId { get; set; }

        [Required, MaxLength(255), Index]
        public string Url { get; set; }

        [MaxLength(255), Index]
        public string RawUrl { get; set; }

        public BannerPlace Place { get; set; }

        /// <summary>
        /// Место размещения баннера на странице
        /// </summary>
        public enum BannerPlace : byte
        {
            Unknown = 0,

            Top = 1,

            Average = 2,

            Intermediate = 3,

            Right = 4,

            Bottom = 5,

            Left = 6
        }

        public BannerType Type { get; set; }

        public enum BannerType:byte
        {
            Html=0,
            Js=1
        }

        /// <summary>
        /// Количество кликов
        /// </summary>
        public int ClicksCount { get; set; }

        /// <summary>
        /// Количество показов
        /// </summary>
        public int ShowsCount { get; set; }

        /// <summary>
        /// Стоимость цели
        /// </summary>
        public decimal TargetCost { get; set; }

        public decimal CPM { get; set; }

        public string Description { get; set; }
    }
}

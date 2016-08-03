﻿using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST")]
    public class SxSiteTest : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(255), Index]
        public string TitleUrl { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        public bool Show { get; set; }

        public string Rules { get; set; }

        public virtual ICollection<SxSiteTestQuestion> Questions { get; set; } 
        public virtual ICollection<SxSiteTestSubject> Answers { get; set; }

        public SiteTestType Type { get; set; }

        public enum SiteTestType : byte
        {
            /// <summary>
            /// Угадыватель
            /// </summary>
            Guess=0,

            /// <summary>
            /// Обычный тест (один вопрос - несколько вариантов ответов)
            /// </summary>
            Normal=1,

            /// <summary>
            /// Обычный тест (один вопрос - несколько вариантов ответов, только в качестве объектов используется картинка)
            /// </summary>
            NormalImage = 2
        }
    }
}

using System;

namespace SX.WebCore
{
    public static class Enums
    {
        public enum ModelCoreType : byte
        {
            Unknown = 0,
            Article = 1,
            News = 2,
            ForumTheme = 3,
            Manual = 4,
            ProjectStep = 5,
            //custom, not for core
            [Obsolete("Не должен поддерживаться в движке")]
            Aphorism = 6,
            [Obsolete("Не должен поддерживаться в движке")]
            Humor =7
        }
    }
}

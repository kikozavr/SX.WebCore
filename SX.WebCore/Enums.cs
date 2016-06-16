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
            Aphorism = 6
        }

        public enum UserClickType : byte
        {
            Unknown = 0,

            /// <summary>
            /// Клик по ссылке поделиться
            /// </summary>
            Share = 1,

            /// <summary>
            /// Лайк по материалу
            /// </summary>
            Like = 2
        }

        public enum UserClickResult : byte
        {
            Unknown = 0,

            /// <summary>
            /// Уже кликнул
            /// </summary>
            Already = 1,

            /// <summary>
            /// Клик успешно проведен
            /// </summary>
            Ok = 2
        }

        public enum LikeDirection : byte
        {
            Unknown = 0,

            /// <summary>
            /// Положительный лайк
            /// </summary>
            Up = 1,

            /// <summary>
            /// Отрицательный лайк
            /// </summary>
            Down = 2
        }
    }
}

namespace SX.WebCore.Abstract
{
    public interface IHasLikes
    {
        int LikeUpCount { get; set; }

        int LikeDownCount { get; set; }
    }
}

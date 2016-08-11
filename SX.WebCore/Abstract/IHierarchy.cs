namespace SX.WebCore.Abstract
{
    public interface IHierarchy<TViewModel>
    {
        string Id { get; set; }

        string ParentCategoryId { get; set; }

        string Title { get; set; }

        int Level { get; set; }

        TViewModel[] ChildCategories { get; set; }
    }
}

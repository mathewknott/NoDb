namespace NoDb.Web.Models.Cues
{
    public interface ICategory
    {
        int Id { get; set; }
        string Title { get; set; }
        string InternalName { get; set; }
        int ParentCategoryId { get; set; }
    }
}
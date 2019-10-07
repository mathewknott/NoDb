namespace Mat.Web.Models.Cues
{
    public interface IQuestion
    {
        int Id { get; set; }
        int Sequence { set; }
        string InternalName { get; set; }
        string Title { get; set; }
        string Answer { get; set; }
        int? CategoryId { get; set; }
    }
}
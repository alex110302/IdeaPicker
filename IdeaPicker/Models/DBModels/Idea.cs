namespace IdeaPicker.Models.DBModels;

public class Idea
{
    [PrimaryKey, NotNull, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Description { get; set; }
    [NotNull] public int TopicId { get; set; }
}
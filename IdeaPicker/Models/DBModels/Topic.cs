namespace IdeaPicker.Models.DBModels;

public class Topic
{
    [PrimaryKey, NotNull, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Name { get; set; }
    
}
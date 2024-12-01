namespace IdeaPicker.Models;

public static class Repository
{
    private static readonly SQLiteConnection _database;

    public static List<Topic> CashedTopics { get; set; }
    public static List<Idea> CashedIdeas { get; set; }

    static Repository()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ideas.db");
        _database = new SQLiteConnection(dbPath);
    }

    public static void SetData(bool seedData = false, bool clearData = false)
    {
        if (clearData)
        {
            _database.DropTable<Idea>();
            _database.DropTable<Topic>();
        }
        
        _database.CreateTable<Idea>();
        _database.CreateTable<Topic>();
        
        if (seedData) SeedData();

        CashedTopics = GetTopicsList();
        CashedIdeas = GetIdeaList();
    }
    
    public static  List<Topic> GetTopicsList()
    {
        return _database.Table<Topic>().ToList();
    }

    public static List<Idea> GetIdeaList()
    {
        return _database.Table<Idea>().ToList();
    }

    public static void SetTopic(Topic topic)
    {
        _database.Insert(topic);
    }

    public static void SetIdea(Idea idea)
    {
        _database.Insert(idea);
    }

    public static void UpdateTopic(Topic topic)
    {
        _database.Update(topic);
        CashedTopics = GetTopicsList();
    }

    public static void UpdateIdea(Idea idea)
    {
        _database.Update(idea);
        CashedIdeas = GetIdeaList();
    }
    
    public static void DeleteTopic(Topic topic)
    {
        _database.Delete<Topic>(topic.Id);
        
        foreach (Idea idea in CashedIdeas) if (idea.TopicId == topic.Id) DeleteIdea(idea);
        
        CashedTopics.Remove(topic);
    }

    public static void DeleteIdea(Idea idea)
    {
        _database.Delete<Idea>(idea.Id);
        CashedIdeas = GetIdeaList();
    }
    
    public static List<Idea> GetSpecificIdeaList(int topicId)
    {
        if (topicId == 0) throw new Exception("The topic was 0 this can not be done!");
        
        List<Idea> newIdeaList = new List<Idea>();
        foreach (Idea idea in CashedIdeas) if (idea.TopicId == topicId) newIdeaList.Add(idea);
        return newIdeaList;
    }

    public static Topic GetSpecificTopic(int topicId)
    {
        return CashedTopics.Find(t => t.Id == topicId);
    }
    
    private static void SeedData()
    {
        
        SetIdea(new Idea
        {
            Description = "Going to McDonold's",
            TopicId = 1
        });
        SetIdea(new Idea
        {
            Description = "Going to Culver's",
            TopicId = 1
        });
        SetIdea(new Idea
        {
            Description = "Going to Taco Bell",
            TopicId = 1
        });

        SetTopic(new Topic()
        {
            Name = "Where to Eat",
        });
        
        SetIdea(new Idea
        {
            Description = "Mr Robot",
            TopicId = 2
        });
        SetIdea(new Idea
        {
            Description = "THe Boys",
            TopicId = 2
        });
        SetIdea(new Idea
        {
            Description = "JoJo's Bizarre Adventure",
            TopicId = 2
        });

        SetTopic(new Topic()
        {
            Name = "What Show to Watch",
        });
    }
    
}
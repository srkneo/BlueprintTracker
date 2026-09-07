namespace BlueprintApi;

public class StudyModule
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
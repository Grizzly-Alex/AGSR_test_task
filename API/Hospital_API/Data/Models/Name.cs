namespace Hospital_API.Data.Models;

public class Name : BaseEntity<Guid>
{
    public string Use { get; init; }
    public string Family { get; init; }
    public ICollection<string> Given { get; init; }
    public Patient Patient { get; init; }
    public Guid PatientId { get; init; }
}

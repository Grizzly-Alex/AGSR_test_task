using Hospital_API.Constants;

namespace Hospital_API.Data.Models;

public class Patient : BaseEntity<Guid>
{
    public Name Name { get; set; }
    public Genders Gender { get; set; }
    public DateTime BirthDate { get; init; }
    public bool Active { get; init; }
}

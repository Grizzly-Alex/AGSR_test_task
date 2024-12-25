namespace ConsoleApp.Models;

public class Patient
{
    public PatientName Name { get; set; }
    public Genders Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Active { get; set; }
}

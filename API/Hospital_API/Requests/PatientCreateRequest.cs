using Hospital_API.Constants;
using System.ComponentModel.DataAnnotations;


namespace Hospital_API.Requests
{
    public class PatientCreateRequest
    {
        public NameCreateRequest Name { get; init; }
        public Genders Gender { get; init; }
        [Required(ErrorMessage = $"BirthDate is required")]
        public DateTime BirthDate { get; init; }
        public bool Active { get; init; }
    }
}

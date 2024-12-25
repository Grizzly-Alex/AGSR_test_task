using System.ComponentModel.DataAnnotations;

namespace Hospital_API.Requests
{
    public class NameUpdateRequest
    {
        [Required(ErrorMessage = $"Id is required")]
        public Guid Id { get; init; }
        public string Use { get; init; }
        [Required(ErrorMessage = $"Family is required")]
        public string Family { get; init; }
        public List<string> Given { get; init; }
    }
}

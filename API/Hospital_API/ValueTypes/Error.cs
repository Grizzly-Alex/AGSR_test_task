using System.Net;

namespace Hospital_API.ValueTypes
{
    public record Error(string messageError)
    {
        public static Error None = new(string.Empty);

        public static Error NullValue = new("Null value was provided");
    }
}

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Hospital_API.Data.Converters;

public class CollectionToJsonConverter<T> : ValueConverter<ICollection<T>, string>
{
    public CollectionToJsonConverter() : base(
        e => JsonConvert.SerializeObject(e.Select(e => e)),
        j => JsonConvert.DeserializeObject<ICollection<T>>(j))
    {

    }
}

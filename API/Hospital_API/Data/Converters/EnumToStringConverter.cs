using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Hospital_API.Data.Converters;

public class EnumToStringConverter<TEnum> : ValueConverter<TEnum, string>
    where TEnum : Enum
{
    public EnumToStringConverter() : base(
        e => JsonConvert.SerializeObject(e.ToString().ToLower()),
        j => JsonConvert.DeserializeObject<TEnum>(j))
    {

    }
}

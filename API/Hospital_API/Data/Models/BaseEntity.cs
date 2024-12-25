using Hospital_API.Interfaces;

namespace Hospital_API.Data.Models;

public class BaseEntity<T> : IAggregateRoot
    where T : struct
{
    public T Id { get; init; }
}

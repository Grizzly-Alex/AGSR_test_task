﻿using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hospital_API.Data.Comparers;

public class CollectionValueComparer<T> : ValueComparer<ICollection<T>>
{
    public CollectionValueComparer() : base((c1, c2) => c1.SequenceEqual(c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToHashSet())
    { }
}

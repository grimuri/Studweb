﻿using Newtonsoft.Json;
using Studweb.Domain.Common.Primitives;

namespace Studweb.Domain.Aggregates.Users.ValueObjects;


public sealed class UserId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private UserId(int value)
    {
        Value = value;
    }

    public static UserId Create(int id) => new UserId(id);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
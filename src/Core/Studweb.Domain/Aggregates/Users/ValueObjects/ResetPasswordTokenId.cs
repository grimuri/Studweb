﻿using Newtonsoft.Json;
using Studweb.Domain.Common.Primitives;

namespace Studweb.Domain.Aggregates.Users.ValueObjects;

public class ResetPasswordTokenId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private ResetPasswordTokenId(int value = default)
    {
        Value = value;
    }

    public static ResetPasswordTokenId Create(int id = default) => new ResetPasswordTokenId(id);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
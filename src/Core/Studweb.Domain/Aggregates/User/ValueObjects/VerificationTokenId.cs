﻿using Newtonsoft.Json;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.User.ValueObjects;

public class VerificationTokenId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private VerificationTokenId(int value = default)
    {
        Value = value;
    }

    public static VerificationTokenId Create(int id = default) => new VerificationTokenId(id);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
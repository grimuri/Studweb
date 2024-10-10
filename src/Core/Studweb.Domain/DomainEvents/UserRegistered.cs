using Studweb.Domain.Entities;
using Studweb.Domain.Entities.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.DomainEvents;

public sealed record UserRegistered(User User) : IDomainEvent;
using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.DomainEvents;

public sealed record UserRegistered(User User) : IDomainEvent;
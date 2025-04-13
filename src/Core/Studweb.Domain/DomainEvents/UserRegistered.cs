using Studweb.Domain.Aggregates.Users;
using Studweb.Domain.Common.Primitives;

namespace Studweb.Domain.DomainEvents;

public sealed record UserRegistered(User User) : IDomainEvent;
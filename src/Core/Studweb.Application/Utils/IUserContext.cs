﻿namespace Studweb.Application.Utils;

public interface IUserContext
{
    bool? IsAuthenticated { get; }
    int? UserId { get; }
}
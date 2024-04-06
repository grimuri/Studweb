namespace Studweb.Application.Common;

public class Result
{
    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None
            || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error");
        }
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T>
{
    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None
            || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error");
        }
        IsSuccess = isSuccess;
        Error = error;
    }
    
    private Result(bool isSuccess, T response)
    {
        if (isSuccess && response == null
            || !isSuccess && response != null)
        {
            throw new ArgumentException("Invalid error");
        }
        IsSuccess = isSuccess;
        Response = response;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public T Response { get; }

    public static Result<T> Success(T response) => new(true, response);
    public static Result<T> Failure(Error error) => new(false, error);
}
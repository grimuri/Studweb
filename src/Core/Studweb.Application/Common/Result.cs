namespace Studweb.Application.Common
{
    public class Result<T>
    {
        private Result(bool isSuccess, T response, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error");
            }

            IsSuccess = isSuccess;
            Response = response;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public T? Response { get; }

        public static Result<T> Success(T response) => new Result<T>(true, response, Error.None);
        public static Result<T> Failure(Error error) => new Result<T>(false, default, error);
        
    }

    public class Result
    {
        private Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);
    }
}
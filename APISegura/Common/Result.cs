namespace APISegura.Common
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public string? Error { get; private set; }
        public T? Data { get; private set; }

        public static Result<T> Ok(T data) =>
            new() { Success = true, Data = data };

        public static Result<T> Fail(string error) =>
            new() { Success = false, Error = error };
    }

    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public static Result Success() =>
            new Result { IsSuccess = true };

        public static Result Failure(string message) =>
            new Result { IsSuccess = false, Message = message };
    }
}

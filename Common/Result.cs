namespace FinTrackHub.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T? Data { get; private set; }

        public static Result<T> Success(T data, string message = "")
            => new Result<T> { IsSuccess = true, Data = data, Message = message };

        public static Result<T> Failure(string message)
            => new Result<T> { IsSuccess = false, Message = message };
    }



}

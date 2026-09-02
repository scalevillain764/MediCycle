using ErrorType = Domain.Enums.ErrorType;
namespace Infrastructure.Responding
{
    public class Result<T> where T: class
    {
        public bool IsSuccess { get; set; }
        public T? Content { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorType? ErrorType { get; set; }
        public Result(bool success, T? content, string? errorMessage, ErrorType? errorType)
        {
            IsSuccess = success;
            Content = content;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
        public static Result<T> Success(T data) => new Result<T>(true, data, null, null);
        public static Result<T> Error(string message, ErrorType type) => new Result<T>(false, null, message, type);
    }
}
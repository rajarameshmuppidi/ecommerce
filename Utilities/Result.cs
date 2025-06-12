namespace EcommercePlatform.Utilities
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public T? Data { get; set; }

        public static Result<T> Success(T data)
        {
            return new()
            {
                Data = data,
                IsSuccess = true
            };
        }

        public static Result<T> Failure(string error)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }
}

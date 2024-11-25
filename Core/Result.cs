namespace Core;

public class Result(bool success, string? message)
{
    public bool Success { get; init; }
    public string Message { get; init; }

    public static Result<T> FromSuccess<T>(T obj, string? message = null)
    {
        return new Result<T>(obj, true, message);
    }

    public static Result FromException(string? message = null)
    {
        return new Result(false, message);
    }
}

public class Result<T>(T obj, bool success, string? message) : Result(success, message)
{
    public T Value { get; init; } = obj;
}

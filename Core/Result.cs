namespace Core;

public class Result(bool success, string? message)
{
    public bool    Success { get; init; } = success;
	public string? Message { get; init; } = message;

    public static Result<T> FromSuccess<T>(T obj, string? message = null) => new(obj, true, message);

    public static Result FromSuccess(string? message = null) => new(true, message);

    public static Result<T> FromException<T>(T? obj = default, string? message = null) => new(obj, false, message);

    public static Result FromException(string? message = null) => new(false, message);
}

public class Result<T>(T? obj, bool success, string? message) : Result(success, message)
{
	public T? Value { get; init; } = obj;
}

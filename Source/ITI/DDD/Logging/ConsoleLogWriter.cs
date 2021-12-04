namespace ITI.DDD.Logging;

public class ConsoleLogWriter : ILogWriter
{
    public static void ClearErrors()
    {
        HasErrors = false;
    }

    public static bool HasErrors { get; protected set; } = false;

    public void Write(
        string? level,
        string? userId,
        string? userName,
        string? hostname,
        string? process,
        string? message,
        Exception? exception
    )
    {
        if (exception != null || level?.ToUpper() == "ERROR")
            HasErrors = true;

        Console.WriteLine($"{DateTime.Now}: {level}: {userId}|{userName}|{hostname}|{process}: {message}");
        if (exception != null)
            Console.WriteLine($"EXCEPTION: {exception}");
    }
}

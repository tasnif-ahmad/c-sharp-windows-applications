using System;
using System.Diagnostics;
using System.IO;

namespace SofiyaBotApplication;

public static class Logger
{
    private static readonly string logFolder =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

    private static readonly string logFile =
        Path.Combine(logFolder, $"log_{DateTime.Now:yyyyMMdd}.txt");

    static Logger()
    {
        if (!Directory.Exists(logFolder))
            Directory.CreateDirectory(logFolder);
    }

    public static void Info(string message)
    {
        Write("INFO", message);
    }

    public static void Error(string message, Exception ex = null)
    {
        Write("ERROR", $"{message} {ex}");
    }

    private static void Write(string level, string message)
    {
        string log = $"{DateTime.Now:HH:mm:ss} [{level}] {message}";

        Debug.WriteLine(log);

        try
        {
            File.AppendAllText(logFile, log + Environment.NewLine);
        }
        catch
        {
            // never crash app because logger failed
        }
    }
}

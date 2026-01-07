using Autodesk.Revit.DB;
using Iter3Task10.Abstraction;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace Iter3Task10.Services
{
    public class LoggerService : ILoggerService, IDisposable
    {
        private readonly Document _document;
        private readonly ILogger _loggerService;
        private readonly string _loggerDirectory;

        public LoggerService(Document document)
        {
            _document=document;
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _loggerDirectory = Path.Combine(appData, "Iter3Task10", "Logs");
            Directory.CreateDirectory(_loggerDirectory);

            var loggerPath = Path.Combine(_loggerDirectory, "app-.log");

            _loggerService = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path: loggerPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                //.WriteTo.Debug()
                .CreateLogger();

            LogInformation($"Logger initialized. Revit document: {_document.PathName}");                
        }

        public void LogDebug(string message) => _loggerService.Debug(message);
        public void LogInformation(string message) => _loggerService.Information(message);
        public void LogWarning(string message) => _loggerService.Warning(message);
        public void LogError(string message) => _loggerService.Error(message);        
        public void LogError(Exception exception, string message) => _loggerService.Error(exception, message);
        public void LogRevitOperation(string operation, string elementID = null)
        {
            var msg = elementID != null
                ? $"Revit operation: {operation} | Element: {elementID}"
                : $"Revit operation: {operation}";
            _loggerService.Information(msg);
        }


        public void Dispose()
        {
            LogInformation("Logger disposed");
            (_loggerService as IDisposable)?.Dispose();
        }
    }
}

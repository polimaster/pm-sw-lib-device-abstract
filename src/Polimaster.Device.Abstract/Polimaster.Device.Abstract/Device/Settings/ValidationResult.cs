using System;

namespace Polimaster.Device.Abstract.Device.Settings; 

/// <summary>
/// Validation result
/// </summary>
public class ValidationResult {
    /// <summary>
    /// Result message
    /// </summary>
    public string Message { get; }
    /// <summary>
    /// Result exception
    /// </summary>
    public Exception? Exception { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">Result message</param>
    /// <param name="exception">Result exception</param>
    public ValidationResult(string message, Exception? exception = null) {
        Message = message;
        Exception = exception;
    }
}
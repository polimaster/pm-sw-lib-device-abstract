namespace Polimaster.Device.Abstract.Transport.Http;

/// <summary>
/// Http client connection parameters
/// </summary>
public struct HttpConnectionParams {
    /// <summary>
    /// Port
    /// </summary>
    public int Port;

    /// <summary>
    /// IP address
    /// </summary>
    public string Ip;

    /// <summary>
    /// Connection timeout
    /// </summary>
    public int Timeout;
}
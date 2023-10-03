using System.Net.Sockets;
using System.Net;
using System.Text;

var address = new IPEndPoint(IPAddress.Any, 8888);
var listener = new TcpListener(address);

try
{
    listener.Start();

    using TcpClient handler = await listener.AcceptTcpClientAsync();
    await using NetworkStream stream = handler.GetStream();

    while(handler.Connected)
    {
        var message = $"{DateTime.Now}";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(messageBytes);
        Console.WriteLine($"Sent message: {message}");
    }
    
}
finally
{
    listener.Stop();
}

//https://learn.microsoft.com/pt-br/dotnet/fundamentals/networking/sockets/tcp-classes

using System.Net.Sockets;
using System.Net;
using System.Text;

var address = new IPEndPoint(IPAddress.Any, 8888);
var listener = new TcpListener(address);

var threads = new Thread[Environment.ProcessorCount];
var cancellationToken = default(CancellationToken);

try
{
    listener.Start();
    for (int i = 0; i < Environment.ProcessorCount; i++)
    {
        var thread = new Thread(async () => {
            var nome = $"Thread-{i}";
            using TcpClient handler = await listener.AcceptTcpClientAsync();
            await using NetworkStream stream = handler.GetStream();
            var buffer = new byte[4096];
            while (handler.Connected)
            {
                var message = Encoding.UTF8.GetBytes(nome);
                await stream.ReadAsync(buffer, 0, buffer.Length);
                await stream.WriteAsync(buffer, cancellationToken);
            }
        });

        threads[i] = thread;
        threads[i].Start();
    }
    
    Console.ReadLine();
}
finally
{
    listener.Stop();
}

//https://learn.microsoft.com/pt-br/dotnet/fundamentals/networking/sockets/tcp-classes

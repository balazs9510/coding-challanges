using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatServer
{
    public class ServerProcess
    {
        private readonly TcpListener _server;
        private readonly ILogger<ServerProcess> _logger;
        private readonly List<TcpClient> _clients = new List<TcpClient>();

        public ServerProcess(ILogger<ServerProcess> logger, string address = "0.0.0.0", int port = 7007)
        {
            var localAddr = IPAddress.Parse(address);
            _server = new TcpListener(localAddr, port);
            _logger = logger;
        }

        public async Task RunServer(CancellationToken cancellationToken)
        {
            try
            {
                _server.Start();
                _logger.LogInformation($"Server started on: {_server.LocalEndpoint}");
                await Task.Run(async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (_server.Pending())
                        {
                            _clients.Add(await _server.AcceptTcpClientAsync());
                            _logger.LogInformation("Client connected.");
                        }

                        await SpreadClientMessages();
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occured.");
            }
            finally
            {
                _server.Stop();
                _logger.LogInformation($"Server stopped.");
            }
        }

        private async Task SpreadClientMessages()
        {
            foreach (var client in _clients.Where(x => x.Connected))
            {
                var stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read > 0)
                    {
                        string data = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        Console.WriteLine($"Received from {client.Client.RemoteEndPoint}: {data}");
                        // Process the received data and prepare a response
                        byte[] responseBytes = Encoding.UTF8.GetBytes(data);

                        // Send the response to all connected clients
                        foreach (TcpClient otherClient in _clients)
                        {
                            if (/*otherClient != client &&*/ otherClient.Connected)
                            {
                                await otherClient.GetStream().WriteAsync(responseBytes, 0, responseBytes.Length);
                            }
                        }
                    }
                }
            }
        }
    }
}

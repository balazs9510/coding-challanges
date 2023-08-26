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

        public ServerProcess(ILogger<ServerProcess> logger, string address = "127.0.0.1", int port = 7007)
        {
            var localAddr = IPAddress.Parse(address);
            _server = new TcpListener(localAddr, port);
            _logger = logger;
        }

        public async Task RunServer()
        {
            try
            {
                _server.Start();
                _logger.LogInformation($"Server started on: {_server.LocalEndpoint}");
                await Task.Run(async () =>
                {
                    while (true)
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
            }
        }

        private async Task SpreadClientMessages()
        {
            foreach (var client in _clients)
            {
                using var stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Received from {client.Client.RemoteEndPoint}: {data}");

                        // Process the received data and prepare a response
                        string response = $"Response to '{data}'\r\n";
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                        // Send the response to all connected clients
                        foreach (TcpClient otherClient in _clients)
                        {
                            if (otherClient != client && otherClient.Connected)
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

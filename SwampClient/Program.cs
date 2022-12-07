using System.Net.Sockets;
using System.Text;

using TcpClient tcpClient = new TcpClient();
await tcpClient.ConnectAsync("", 8888);

var stream = tcpClient.GetStream();
var response = await GetServerResponse();

var byteMessages = Encoding.UTF8.GetString(response.ToArray());

var messages = byteMessages.Split(new char[] { '@' }).ToList();

foreach (var message in messages)
{
    Console.WriteLine(message);
}

string input = Console.ReadLine();

await stream.WriteAsync(Encoding.UTF8.GetBytes($"{input}#"));
while (true)
{
    Console.WriteLine();
    var responce = Console.ReadLine();
    await stream.WriteAsync(Encoding.UTF8.GetBytes($"{responce}#"));
}

async Task<List<byte>> GetServerResponse()
{
    var response = new List<byte>();
    int bytesRead = 10; // для считывания байтов из потока

    while ((bytesRead = stream.ReadByte()) != '#')
    {
        // добавляем в буфер
        response.Add((byte)bytesRead);
    }

    return response;
}
using System.Net.Sockets;
using System.Text;

var stream = await GetStream();
var response = await GetServerResponse();

DisplayResponce(response);

while (true)
{
    Console.WriteLine("Press Enter for proceed. Send \"q\" for the quit.");
    var responce = Console.ReadLine();
    await stream.WriteAsync(Encoding.UTF8.GetBytes($"{responce}#"));
}

async Task<NetworkStream> GetStream()
{
    TcpClient tcpClient = new TcpClient();
    await tcpClient.ConnectAsync("192.168.0.53", 8888);

    return tcpClient.GetStream();
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

void DisplayResponce(List<byte> receivedResponse)
{
    var byteMessages = Encoding.UTF8.GetString(receivedResponse.ToArray());

    var messages = byteMessages.Split(new char[] { '@' }).ToList();

    foreach (var message in messages)
    {
        Console.WriteLine(message);
    }
}
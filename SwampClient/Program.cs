using System.Net.Sockets;
using System.Text;

string exMessage = "";
NetworkStream stream = null;
try
{
    stream = await GetStream();
    var response = await GetServerResponse();
    bool firstTime = true;

    DisplayResponce(response);

    while (true)
    {
        if (!firstTime)
            Console.WriteLine("Press Enter for proceed. Send \"q\" for the quit.");
        else
            firstTime = false;

        var responce = Console.ReadLine();
        await stream.WriteAsync(Encoding.UTF8.GetBytes($"{responce}#"));
    }
}
catch (Exception ex)
{
    exMessage = ex.Message;
}
finally
{
    StreamWriter sw = new StreamWriter("log.txt");
    sw.Write(exMessage);
    sw.Close();
}

async Task<NetworkStream> GetStream()
{
    TcpClient tcpClient = new TcpClient();
    await tcpClient.ConnectAsync("192.168.0.53", 8887);

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
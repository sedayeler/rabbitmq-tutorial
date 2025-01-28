using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "header-exchange-example", type: ExchangeType.Headers);

for (int i = 0; i < 50; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");

    Console.Write("Lütfen header value giriniz: ");
    string value = Console.ReadLine();

    var properties = new BasicProperties();
    properties.Headers = new Dictionary<string, object?>()
    {
        ["no"] = value
    };

    await channel.BasicPublishAsync(exchange: "header-exchange-example", routingKey: string.Empty, mandatory: true, basicProperties: properties, body: message); 
    //mandatory: Mesaj yönlendirilmezse publisher'a geri bildirilir.
}

Console.Read();

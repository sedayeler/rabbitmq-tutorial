using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

#region P2P (Point-to-Point) Tasarımı
//await channel.QueueDeclareAsync(queue: "example", durable: false, exclusive: false);

//byte[] message = Encoding.UTF8.GetBytes("Merhaba!");
//await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "example", body: message);
#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı
//string exchangeName = "pub-sub-exchange-example";
//await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout);

//for (int i = 0; i < 50; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");

//    await channel.BasicPublishAsync(exchangeName, string.Empty, message);
//}
#endregion

#region Work Queue(İş Kuyruğu) Tasarımı​
string queueName = "work-queue-example";
await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false);

for (int i = 0; i < 50; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: message);
}
#endregion

#region Request/Response Tasarımı​

#endregion

Console.Read();

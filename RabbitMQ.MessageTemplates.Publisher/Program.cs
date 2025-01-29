using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
//string queueName = "work-queue-example";
//await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false);

//for (int i = 0; i < 50; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

//    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: message);
//}
#endregion

#region Request/Response Tasarımı​
string requestQueueName = "request-queue-example";
await channel.QueueDeclareAsync(requestQueueName, false, false, false);

string replyQueueName = "reply-queue-example";
await channel.QueueDeclareAsync(replyQueueName, false, false, false);

string correlationId = Guid.NewGuid().ToString();

var properties = new BasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

    await channel.BasicPublishAsync(string.Empty, requestQueueName, false, properties, message);
}

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(replyQueueName, false, consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine("Response: " + Encoding.UTF8.GetString(e.Body.Span));
    }

    await channel.BasicAckAsync(e.DeliveryTag, false);
};
#endregion

Console.Read();

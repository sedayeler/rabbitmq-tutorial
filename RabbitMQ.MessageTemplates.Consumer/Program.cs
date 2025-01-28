using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

#region P2P (Point-to-Point) Tasarımı
//await channel.QueueDeclareAsync(queue: "example", durable: false, exclusive: false);

//AsyncEventingBasicConsumer consumer = new(channel);
//await channel.BasicConsumeAsync(queue: "example", autoAck: false, consumer: consumer);

//consumer.ReceivedAsync += async (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

//    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
//};
#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı
//string exchangeName = "pub-sub-exchange-example";
//await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout);

//var queue = await channel.QueueDeclareAsync();
//string queueName = queue.QueueName;

//await channel.QueueBindAsync(queueName, exchangeName, routingKey: string.Empty);

//AsyncEventingBasicConsumer consumer = new(channel);
//await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

//consumer.ReceivedAsync += async (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

//    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
//};
#endregion

#region Work Queue(İş Kuyruğu) Tasarımı​
string queueName = "work-queue-example";
await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false);

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

//await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
};
#endregion

#region Request/Response Tasarımı​

#endregion

Console.Read();
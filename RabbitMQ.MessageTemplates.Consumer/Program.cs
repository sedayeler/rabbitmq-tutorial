using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

#region P2P (Point-to-Point) Tasarımı
//await channel.QueueDeclareAsync(queue: "example", exclusive: false);

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

//await channel.QueueBindAsync(queueName, exchangeName, string.Empty);

//AsyncEventingBasicConsumer consumer = new(channel);
//await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

//consumer.ReceivedAsync += async (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

//    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
//};
#endregion

#region Work Queue(İş Kuyruğu) Tasarımı​
//string queueName = "work-queue-example";
//await channel.QueueDeclareAsync(queue: queueName, exclusive: false);

//AsyncEventingBasicConsumer consumer = new(channel);
//await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

//await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

//consumer.ReceivedAsync += async (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

//    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
//};
#endregion

#region Request/Response Tasarımı​
string requestQueueName = "request-queue-example";
await channel.QueueDeclareAsync(requestQueueName, false, false, false);

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(requestQueueName, false, consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    await channel.BasicAckAsync(e.DeliveryTag, false);

    byte[] responseMessage = Encoding.UTF8.GetBytes("İşlem tamamlandı. " + message);

    BasicProperties properties = new BasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;

    await channel.BasicPublishAsync(string.Empty, e.BasicProperties.ReplyTo, false, properties, responseMessage);
};
#endregion

Console.Read();
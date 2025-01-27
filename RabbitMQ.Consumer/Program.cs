using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "example", durable: true, exclusive: false);

AsyncEventingBasicConsumer consumer = new(channel);
var consumerTag = await channel.BasicConsumeAsync(queue: "example", autoAck: false, consumer: consumer);
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false); //Consumer yük dağılımı
consumer.ReceivedAsync += async (sender, e) =>
{
    //Queue'ya gelen mesajın işlendiği yerdir.
    //e.Body.Span veya e.Body.ToArray(): Kuyruktaki mesajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
    //await channel.BasicNackAsync(deliveryTag: e.DeliveryTag, multiple: false, requeue: true); 
    //await channel.BasicCancelAsync(consumerTag: consumerTag); Kuyruktan gelen tüm mesajları reddeder.
    //await channel.BasicRejectAsync(deliveryTag: 3, requeue: true); Kuyruktan gelen 3.mesajı reddeder.
};

Console.Read();

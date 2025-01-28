using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

Console.Write("Kuyruk adı giriniz: ");
string queueName = Console.ReadLine();

await channel.QueueDeclareAsync(queue: queueName, exclusive: false);

await channel.QueueBindAsync(queue: queueName, exchange: "fanout-exchange-example", routingKey: string.Empty);

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "header-exchange-example", type: ExchangeType.Headers);

Console.Write("Lütfen header value giriniz: ");
string value = Console.ReadLine();

var queue = await channel.QueueDeclareAsync();
string queueName = queue.QueueName;

await channel.QueueBindAsync(queue: queueName,
    exchange: "header-exchange-example",
    routingKey: string.Empty,
    new Dictionary<string, object?>
    {
        ["no"] = value
    });

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    await channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();


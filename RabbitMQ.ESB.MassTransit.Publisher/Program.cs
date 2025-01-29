using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

string rabbitMQUrl = "amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy";

string queueName = "queue-example";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUrl);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new Uri($"{rabbitMQUrl}/{queueName}"));

Console.Write("Gönderilecek mesaj: ");
string message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new ExampleMessage()
{
    Text = message
});

Console.Read();

using MassTransit;
using RabbitMQ.ESB.MassTransit.RequestResponsePattern.Consumer.Consumers;

string rabbitMQUrl = "amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy";

string queueName = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUrl);

    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<RequestMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();

using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;

string rabbitMQUrl = "amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy";

string queueName = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUrl);
});

await bus.StartAsync();

var request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUrl}/{queueName}"));

int i = 1;
while (true)
{
    await Task.Delay(200);
    var response = await request.GetResponse<ResponseMessage>(new()
    {
        MessageNo = i,
        Text = $"{i++}. request"
    });
    Console.WriteLine($"Response received: {response.Message.Text}");
}





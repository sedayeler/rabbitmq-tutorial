using RabbitMQ.Client;
using System.Text;

//Bağlantı oluşturma
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://uilltroy:fF8_cjK6zL2ia3G5S3NIUQurkhQsx7yj@jackal.rmq.cloudamqp.com/uilltroy");

//Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

//Queue oluşturma
await channel.QueueDeclareAsync(queue: "example", durable: true, exclusive: false);

//Queue'ya mesaj gönderme
//RabbitMQ kuyruktaki mesajları byte türünde kabul etmektedir. 
byte[] message = Encoding.UTF8.GetBytes("Merhaba");
await channel.BasicPublishAsync(exchange: "", routingKey: "example", body: message);

Console.Read();



using RabbitMQ.Client;
using System.Text;

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://xtnimwcx:XWIcS9Ni0pikXFBMkKVvzEsKEWZ4NB8N@whale.rmq.cloudamqp.com/xtnimwcx");

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

channel.QueueDeclare("first-messages", true, false, false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    string message = $"This is message of {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "first-messages", null, messageBody);
    Console.WriteLine($"Message is sent :{message} ");
});

Console.ReadLine();



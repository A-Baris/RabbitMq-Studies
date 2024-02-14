
using Producer;
using RabbitMQ.Client;
using System.Text;




var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://xtnimwcx:XWIcS9Ni0pikXFBMkKVvzEsKEWZ4NB8N@whale.rmq.cloudamqp.com/xtnimwcx");

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogName)).ToList().ForEach(x =>
{
    var queueName = $"direct-queue-{x}";
    var routeKey = $"route-{x}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, "logs-direct", routeKey,null);

});




Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogName logName = (LogName)new Random().Next(1, 5);
    string message = $"log-type:{logName}";
    

    var messageBody = Encoding.UTF8.GetBytes(message);
    var routeKey = $"route-{logName}";
    channel.BasicPublish("logs-direct", routeKey,null, messageBody);
    Console.WriteLine($"Log is sent :{message} ");
});

Console.ReadLine();



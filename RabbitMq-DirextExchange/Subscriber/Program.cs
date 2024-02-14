
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://xtnimwcx:XWIcS9Ni0pikXFBMkKVvzEsKEWZ4NB8N@whale.rmq.cloudamqp.com/xtnimwcx");

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();



channel.BasicQos(0, 1, false);
var subscriber = new EventingBasicConsumer(channel);

var queueName = "direct-queue-Critical";
channel.BasicConsume(queueName, false, subscriber);
Console.WriteLine("listening to logs...");

subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(2000);
    Console.WriteLine($"Received message: {message}");
    File.AppendAllText("log-Critical.txt", message + "\n");
    channel.BasicAck(e.DeliveryTag, false);


};
Console.ReadLine();

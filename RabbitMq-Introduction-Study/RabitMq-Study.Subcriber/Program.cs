
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://xtnimwcx:XWIcS9Ni0pikXFBMkKVvzEsKEWZ4NB8N@whale.rmq.cloudamqp.com/xtnimwcx");

using var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

//var randomQueue = channel.QueueDeclare().QueueName;
var randomQueue = "logs-save-queue";
channel.QueueDeclare(randomQueue,true,false,false);

channel.QueueBind(randomQueue, "logs-fanout", "", null); 

channel.BasicQos(0,1,false);
var subscriber = new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueue, false,subscriber);
Console.WriteLine("listening to logs...");

subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(2000);
    Console.WriteLine($"Received message: {message}");

    channel.BasicAck(e.DeliveryTag, false);


};
Console.ReadLine();
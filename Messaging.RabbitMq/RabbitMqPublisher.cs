using System.Text;

namespace Messaging.RabbitMq
{
	public sealed class RabbitMqPublisher
	{
		public void Publish(string message)
		{
			var rabbitMqConnection = new RabbitMqConnection();
			
			var channel = rabbitMqConnection.Connect();
			var body = Encoding.UTF8.GetBytes(message);
			
			var properties = channel.CreateBasicProperties();
			properties.Persistent = true;

			channel.BasicPublish(Constants.Exchange, Constants.Key, true, properties, body);
			rabbitMqConnection.Disconnect();
		}
	}
}

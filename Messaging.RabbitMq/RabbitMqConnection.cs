using RabbitMQ.Client;

namespace Messaging.RabbitMq
{
	public sealed class RabbitMqConnection
	{
		private IConnection _connection;
		private IModel _channel;

		internal IModel Connect()
		{
			var factory = new ConnectionFactory
			{
				HostName = Constants.HostName,
				VirtualHost = Constants.VirtualHost,
				UserName = Constants.UserName,
				Password = Constants.Password,
				RequestedHeartbeat = 30
			};

			// Get connection to RabbitMQ
			_connection = factory.CreateConnection();

			// Get reference to channel
			_channel = _connection.CreateModel();

			// Fetch one message at a time.
			_channel.BasicQos(0, 1, false);

			// Create exchange
			_channel.ExchangeDeclare(Constants.Exchange, "direct", durable: true);

			// Create queue
			_channel.QueueDeclare(Constants.Queue, durable: true, exclusive: false, autoDelete: false);

			// Bind exchange to queue
			_channel.QueueBind(Constants.Queue, Constants.Exchange, Constants.Key);

			return _channel;
		}

		internal void Disconnect()
		{
			if (_channel != null)
			{
				_channel.Dispose();
			}

			if (_connection != null)
			{
				_connection.Dispose();
				_connection = null;
			}
		}
	}
}

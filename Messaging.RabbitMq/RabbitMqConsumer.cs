using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.RabbitMq
{
	public sealed class RabbitMqConsumer
	{
		private readonly RabbitMqConnection _rabbitMqConnection;
		private IModel _channel;
		private readonly Random _random;

		public event EventHandler OnEvent;

		public RabbitMqConsumer()
		{
			_rabbitMqConnection = new RabbitMqConnection();
			_random = new Random();
		}

		public void Subscribe()
		{
			try
			{
				LogMessage($"Trying to connect...");
				_channel = _rabbitMqConnection.Connect();
				var consumer = new EventingBasicConsumer(_channel);
				consumer.Received += (model, eventArgs) => MessageReceived(model, eventArgs);
				_channel.BasicConsume(
					queue: Constants.Queue,
					autoAck: false,
					consumerTag: string.Empty,
					noLocal: false,
					exclusive: false,
					arguments: new Dictionary<string, object>(),
					consumer: consumer);
				LogMessage($"Connected");
			}
			catch (Exception ex)
			{
				LogMessage($"Error connecting: {ex.Message}");
			}
		}

		private void MessageReceived(object model, BasicDeliverEventArgs eventArgs)
		{
			try
			{
				LogMessage("Got new message:");
				var message = Encoding.Default.GetString(eventArgs.Body);
				LogMessage($"- {message}");
				LogMessage($"- Message received at {DateTime.Now:hh:mm:ss.fff}; starting work...");
				Thread.Sleep(_random.Next(2000, 5000));
				LogMessage($"- Done working at {DateTime.Now:hh:mm:ss.fff}");
				_channel.BasicAck(eventArgs.DeliveryTag, false);
			}
			catch (Exception ex)
			{
				LogMessage($"Error reading message: {ex.Message}");
			}
		}

		private void LogMessage(string message)
		{
			OnEvent?.Invoke(this, new ConsumerArgs(message));
		}
	}

	public sealed class ConsumerArgs : EventArgs
	{
		public readonly string Message;

		public ConsumerArgs(string message)
		{
			Message = message;
		}
	}
}

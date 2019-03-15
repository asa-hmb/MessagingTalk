using System;
using System.Windows;
using Messaging.RabbitMq;

namespace Messaging.Publisher
{
	public partial class MainWindow : Window
	{
		private RabbitMqPublisher _publisher;

		public MainWindow()
		{
			InitializeComponent();

			_publisher = new RabbitMqPublisher();
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			var message = $"Button clicked at {DateTime.Now:hh:mm:ss.fff}";
			_publisher.Publish(message);
		}
	}
}

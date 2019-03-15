using System;
using System.Windows;
using Messaging.RabbitMq;

namespace Messaging.Consumer
{
	public partial class MainWindow : Window
	{
		private RabbitMqConsumer _consumer;

		public MainWindow()
		{
			InitializeComponent();
			_consumer = new RabbitMqConsumer();
			_consumer.OnEvent += (sender, args) =>
				Dispatcher.Invoke(() =>
				{
					AppendText(((ConsumerArgs)args).Message);
				});
			_consumer.Subscribe();
		}

		private void AppendText(string text)
		{
			Output.Text += $"{text}{Environment.NewLine}";
			Output.Focus();
			Output.CaretIndex = Output.Text.Length;
			Output.ScrollToEnd();
		}
	}
}

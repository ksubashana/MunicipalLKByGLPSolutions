using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MuniLK.Application.Generic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMqConsumerWorker : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private RabbitMQ.Client.IChannel _channel;
    public RabbitMqConsumerWorker(
        ILogger<RabbitMqConsumerWorker> logger,
         IServiceProvider serviceProvider,
         IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("RabbitMQ Consumer App Started.");
        Console.WriteLine("Waiting for messages. Press [enter] to exit.");

        // Create a ConnectionFactory to establish a connection to RabbitMQ
        // HostName, Port, UserName, and Password should match your RabbitMQ setup.
        // We're using localhost and port 25672 as per your Docker mapping.
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"), // Default RabbitMQ port
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        try
        {
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            string exchangeName = "munilk-logs-exchange";
            string routingKey = "munilk-logs-route";
            string queueName = "munilk-logs-queue"; // You define this

            // Ensure exchange exists
            await _channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            // Declare your own queue
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange and routing key
            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"[x] Received from RabbitMQ: {message}");
                using var scope = _serviceProvider.CreateScope();

                // Resolve the scoped IMyMessageProcessor instance
                var processor = scope.ServiceProvider.GetRequiredService<IMyMessageProcessor>();

                try
                {
                    await processor.ProcessMessageAsync(message);

                    //await _messageProcessor.ProcessMessageAsync(message);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message.");
                    // Optionally reject and requeue or dead-letter
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                }
            };

            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer);

            _logger.LogInformation("RabbitMQ consumer started. Listening for messages...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to consume from RabbitMQ");
        }

    }


    public override void Dispose()
    {
        try
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.CloseAsync().GetAwaiter().GetResult();
            }
            if (_connection != null && _connection.IsOpen)
            {
                _connection.CloseAsync().GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ resources");
        }
        finally
        {
            _channel?.Dispose();
            _channel = null;
            _connection?.Dispose();
            _connection = null;
            base.Dispose();
        }
    }
}

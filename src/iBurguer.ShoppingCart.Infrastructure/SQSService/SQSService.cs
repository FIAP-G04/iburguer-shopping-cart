using Amazon.Runtime;
using Amazon.SQS.Model;
using Amazon.SQS;
using iBurguer.ShoppingCart.Core.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Amazon;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.SQSService
{
    [ExcludeFromCodeCoverage]
    public class SQSService : ISQSService
    {
        private readonly IAmazonSQS _client;
        private readonly string _queue;

        public SQSService(IOptions<SQSConfiguration> options)
        {
            var configuration = options.Value;
            _client = CreateClient(configuration);
            _queue = configuration.Queue;
        }

        public async Task SendMessage(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var queueUrl = await GetQueueUrl(_queue);

            var request = new SendMessageRequest()
            {
                MessageBody = JsonConvert.SerializeObject(domainEvent),
                QueueUrl = queueUrl
            };

            await _client.SendMessageAsync(request, cancellationToken);
        }

        private static IAmazonSQS CreateClient(SQSConfiguration configuration)
        {
            var accessKey = configuration.AccessKey;
            var secretKey = configuration.SecretKey;
            var region = RegionEndpoint.USEast1;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            return new AmazonSQSClient(credentials, region);
        }

        private async Task<string> GetQueueUrl(string queueName)
        {
            var response = await _client.GetQueueUrlAsync(new GetQueueUrlRequest
            {
                QueueName = queueName
            });

            return response.QueueUrl;
        }
    }
}

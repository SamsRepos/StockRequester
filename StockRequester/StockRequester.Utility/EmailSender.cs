using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using StockRequester.Utility.Models;

namespace StockRequester.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<AwsSettings> _awsSettings;

        public EmailSender(IOptions<AwsSettings> awsSettings)
        {
            _awsSettings = awsSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string awsRegion = _awsSettings.Value.Region;
            string awsAccessKey = _awsSettings.Value.AccessKey;
            string awsSecretKey = _awsSettings.Value.SecretKey;

            var credentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKey, awsSecretKey);
            using (var client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.GetBySystemName(awsRegion)))
            {
                SendEmailRequest sendRequest = new SendEmailRequest
                {
                    Source = $"{Consts.STOCK_REQUESTER_EMAIL_NAME} <{Consts.STOCK_REQUESTER_EMAIL_ADDRESS}>",
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { email }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content(htmlMessage)
                        }
                    }
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    Console.WriteLine($"Email sent! Message ID: {response.MessageId}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"The email was not sent. Error message: {ex.Message}");
                }
            }
        }
    }
}

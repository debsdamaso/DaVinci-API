using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _sendGridApiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            // Recupera a chave API e o e-mail do remetente do appsettings.json
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
            _senderEmail = configuration["SendGrid:SenderEmail"];
            _senderName = configuration["SendGrid:SenderName"];
            _logger = logger;
        }

        /// <summary>
        /// Método genérico para enviar um e-mail com assunto e conteúdo especificados.
        /// </summary>
        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string content)
        {
            try
            {
                var client = new SendGridClient(_sendGridApiKey);
                var from = new EmailAddress(_senderEmail, _senderName);
                var to = new EmailAddress(recipientEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    _logger.LogInformation($"E-mail enviado para {recipientEmail} com sucesso.");
                    return true;
                }
                else
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    _logger.LogError($"Erro ao enviar e-mail: StatusCode={response.StatusCode}, Body={responseBody}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar e-mail: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Envia um lembrete de feedback para clientes que compraram produtos mas não deixaram avaliação.
        /// </summary>
        public async Task SendFeedbackReminderAsync(string recipientEmail, string productName)
        {
            var subject = "Gostaria de deixar uma avaliação sobre o produto que comprou?";
            var content = $"Olá! Notamos que você comprou o produto '{productName}' recentemente, mas ainda não avaliou. Sua opinião é muito importante para nós! Por favor, clique no link para deixar seu feedback.";
            await SendEmailAsync(recipientEmail, subject, content);
        }
    }
}

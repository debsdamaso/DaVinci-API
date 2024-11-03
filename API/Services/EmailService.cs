using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Services
{
    public class EmailService : IEmailService // Implemente a interface
    {
        private readonly string _sendGridApiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            // Recupera a chave API e o e-mail do remetente do appsettings.json
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
            _senderEmail = configuration["SendGrid:SenderEmail"];
            _senderName = configuration["SendGrid:SenderName"];
        }

        /// <summary>
        /// Método genérico para enviar um e-mail com assunto e conteúdo especificados.
        /// </summary>
        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string content) // Atualize o tipo de retorno
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress(_senderEmail, _senderName);
            var to = new EmailAddress(recipientEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // Log de erro ou tratamento de erro adicional, caso necessário
                Console.WriteLine("Erro ao enviar e-mail: " + response.StatusCode);
                return false; // Retorna false em caso de erro
            }
            return true; // Retorna true em caso de sucesso
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

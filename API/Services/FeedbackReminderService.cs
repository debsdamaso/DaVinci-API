using System;
using System.Threading.Tasks;
using API.Repositories;
using API.Services;
using API.Models;

namespace API.Services
{
    public class FeedbackReminderService
    {
        private readonly IEmailService _emailService;
        private readonly IPurchaseRepository _purchaseRepository;

        public FeedbackReminderService(IEmailService emailService, IPurchaseRepository purchaseRepository)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _purchaseRepository = purchaseRepository ?? throw new ArgumentNullException(nameof(purchaseRepository));
        }

        // Envia lembretes de feedback para clientes que não deixaram feedback
        public async Task SendFeedbackRemindersAsync()
        {
            var purchasesWithoutFeedback = await _purchaseRepository.GetPurchasesWithoutFeedbackAsync();

            foreach (var purchase in purchasesWithoutFeedback)
            {
                // Validação para garantir que o email do cliente está disponível
                if (string.IsNullOrEmpty(purchase.CustomerEmail))
                {
                    // Logar ou manipular caso o email esteja ausente
                    Console.WriteLine($"Compra com ID {purchase.Id} não possui email do cliente.");
                    continue;
                }

                // Configuração da mensagem de email
                var subject = "Gostaríamos de saber sua opinião!";
                var message = $"Olá! Por favor, deixe sua avaliação sobre o produto que comprou. Sua opinião é muito importante para nós.";

                try
                {
                    await _emailService.SendEmailAsync(purchase.CustomerEmail, subject, message);
                    Console.WriteLine($"Lembrete de feedback enviado para {purchase.CustomerEmail} sobre a compra {purchase.Id}.");
                }
                catch (Exception ex)
                {
                    // Log de erro caso o envio do email falhe
                    Console.WriteLine($"Erro ao enviar email para {purchase.CustomerEmail} sobre a compra {purchase.Id}: {ex.Message}");
                }
            }
        }
    }
}

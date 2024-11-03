using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class FeedbackReminderService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IEmailService _emailService;

        public FeedbackReminderService(IPurchaseRepository purchaseRepository, IEmailService emailService)
        {
            _purchaseRepository = purchaseRepository;
            _emailService = emailService;
        }

        public async Task SendFeedbackRemindersAsync()
        {
            // Obtém todas as compras que não têm feedback
            var purchasesWithoutFeedback = await _purchaseRepository.GetPurchasesWithoutFeedbackAsync();

            foreach (var purchase in purchasesWithoutFeedback)
            {
                if (!string.IsNullOrEmpty(purchase.CustomerEmail))
                {
                    string subject = "Sua opinião é importante para nós!";
                    string content = "Olá, por favor, deixe seu feedback sobre sua última compra.";
                    await _emailService.SendEmailAsync(purchase.CustomerEmail, subject, content);
                }
            }
        }
    }
}
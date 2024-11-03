using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Services;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseRepository _purchaseRepository;
        private readonly FeedbackReminderService _feedbackReminderService;
        private readonly FeedbackRepository _feedbackRepository; // Adicionado para criar feedbacks

        public PurchaseController(PurchaseRepository purchaseRepository, FeedbackReminderService feedbackReminderService, FeedbackRepository feedbackRepository)
        {
            _purchaseRepository = purchaseRepository;
            _feedbackReminderService = feedbackReminderService;
            _feedbackRepository = feedbackRepository;
        }

        // GET: api/purchase
        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> GetAllPurchases()
        {
            var purchases = await _purchaseRepository.GetAllPurchasesAsync();
            return Ok(purchases);
        }

        // GET: api/purchase/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchaseById(string id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }

        // POST: api/purchase
        [HttpPost]
        public async Task<ActionResult<Purchase>> CreatePurchase(Purchase purchase)
        {
            await _purchaseRepository.AddPurchaseAsync(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
        }

        // PUT: api/purchase/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchase(string id, Purchase purchase)
        {
            var existingPurchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (existingPurchase == null)
                return NotFound();

            purchase.Id = id;
            await _purchaseRepository.UpdatePurchaseAsync(purchase);
            return NoContent();
        }

        // DELETE: api/purchase/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(string id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
                return NotFound();

            await _purchaseRepository.DeletePurchaseAsync(id);
            return NoContent();
        }

        // POST: api/purchase/send-feedback-reminders
        [HttpPost("send-feedback-reminders")]
        public async Task<IActionResult> SendFeedbackReminders()
        {
            await _feedbackReminderService.SendFeedbackRemindersAsync();
            return Ok(new { message = "Lembretes de feedback enviados com sucesso." });
        }

        // POST: api/purchase/{id}/add-feedback
        [HttpPost("{purchaseId}/add-feedback")]
        public async Task<IActionResult> AddFeedbackToPurchase(string purchaseId, Feedback feedback)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
            if (purchase == null)
                return NotFound();

            // Se CreateFeedbackAsync não retorna um ID, remova a atribuição a feedbackId
            await _feedbackRepository.CreateFeedbackAsync(feedback);
            // Assumindo que CreateFeedbackAsync apenas cria o feedback e não retorna um ID
            await _purchaseRepository.AssociateFeedbackToPurchaseAsync(purchaseId, feedback.Id); // Use feedback.Id se ele já contiver o ID
            return Ok(new { message = "Feedback associado à compra com sucesso." });
        }

        // GET: api/purchase/pending-feedback
        [HttpGet("pending-feedback")]
        public async Task<ActionResult<List<Purchase>>> GetPurchasesPendingFeedback()
        {
            var purchases = await _purchaseRepository.GetPurchasesWithoutFeedbackAsync();
            return Ok(purchases);
        }
    }
}


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
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly FeedbackReminderService _feedbackReminderService;
        private readonly IFeedbackRepository _feedbackRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository, FeedbackReminderService feedbackReminderService, IFeedbackRepository feedbackRepository)
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
            {
                return NotFound();
            }
            return Ok(purchase);
        }

        // POST: api/purchase
        [HttpPost]
        public async Task<ActionResult> CreatePurchase(Purchase purchase)
        {
            await _purchaseRepository.AddPurchaseAsync(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
        }

        // PUT: api/purchase/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePurchase(string id, Purchase updatedPurchase)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            updatedPurchase.Id = id;
            await _purchaseRepository.UpdatePurchaseAsync(updatedPurchase);
            return NoContent();
        }

        // DELETE: api/purchase/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePurchase(string id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            await _purchaseRepository.DeletePurchaseAsync(id);
            return NoContent();
        }
    }
}
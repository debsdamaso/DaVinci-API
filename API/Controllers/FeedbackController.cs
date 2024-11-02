using Microsoft.AspNetCore.Mvc;
using DaVinci.Models;
using API.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        // GET: api/feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            return Ok(feedbacks);
        }

        // GET: api/feedback/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedbackById(string id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }

        // POST: api/feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> CreateFeedback(Feedback feedback)
        {
            await _feedbackRepository.CreateAsync(feedback);
            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, feedback);
        }

        // PUT: api/feedback/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(string id, Feedback feedback)
        {
            var existingFeedback = await _feedbackRepository.GetByIdAsync(id);
            if (existingFeedback == null)
            {
                return NotFound();
            }

            await _feedbackRepository.UpdateAsync(id, feedback);
            return NoContent();
        }

        // DELETE: api/feedback/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(string id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            await _feedbackRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
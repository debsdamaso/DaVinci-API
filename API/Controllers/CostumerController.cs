using Microsoft.AspNetCore.Mvc;
using DaVinci.Models;
using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostumerController : ControllerBase
    {
        private readonly ICostumerRepository _costumerRepository;

        public CostumerController(ICostumerRepository costumerRepository)
        {
            _costumerRepository = costumerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Costumer>>> GetAll()
        {
            var costumers = await _costumerRepository.GetAllAsync();
            return Ok(costumers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Costumer>> GetById(string id)
        {
            var costumer = await _costumerRepository.GetByIdAsync(id);
            if (costumer == null)
                return NotFound();
            return Ok(costumer);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Costumer costumer)
        {
            await _costumerRepository.CreateAsync(costumer);
            return CreatedAtAction(nameof(GetById), new { id = costumer.Id }, costumer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Costumer costumer)
        {
            var existingCostumer = await _costumerRepository.GetByIdAsync(id);
            if (existingCostumer == null)
                return NotFound();
            await _costumerRepository.UpdateAsync(id, costumer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var existingCostumer = await _costumerRepository.GetByIdAsync(id);
            if (existingCostumer == null)
                return NotFound();
            await _costumerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

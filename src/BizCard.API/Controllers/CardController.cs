using System.Threading.Tasks;
using BizCard.API.ViewModel;
using BizCard.Core.Data;
using BizCard.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizCard.API.Controllers
{
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly IRepository<Card> _cardRepo;

        public CardController(IRepository<Card> cardRepo)
        {
            _cardRepo = cardRepo;
        }

        [HttpGet]
        public async Task<IActionResult> CardInfo()
        {
            return Ok(await _cardRepo.All().ToListAsync());
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> AddCard([FromBody] CardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var card = new Card
            {
                Name = model.Name,
                Title = model.Title,
                Company = model.Company,
                Contact = model.Contact,
                Address = model.Address
            };

            await _cardRepo.Save(card);

            return CreatedAtAction(nameof(CardInfo), new { id = card.Id }, "Card created…");
        }
    }
}

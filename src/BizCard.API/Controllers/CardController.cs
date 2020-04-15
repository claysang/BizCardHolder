using System;
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
        
        [HttpPut]
        [Route("{cardId?}")]
        public async Task<ActionResult<object>> EditCard([FromBody] CardViewModel model, int cardId)
        {

            var card = _cardRepo.Get(cardId);
            
            if (card == null)
            {
                return BadRequest("Card not found…");
            }

            var allCardProperties = typeof(CardViewModel).GetProperties();

            foreach (var property in allCardProperties)
            {
                var propName = property.Name;
                var modelValue = property.GetValue(model);
                
                if (modelValue != null)
                {
                    typeof(Card).GetProperty(propName)?.SetValue(card, modelValue);
                }
            }

            await _cardRepo.Update(card);

            return CreatedAtAction(nameof(CardInfo), new { id = cardId }, card);
        }
        
        [HttpDelete]
        [Route("{cardId?}")]
        public ActionResult<string> DeleteCard(int cardId)
        {

            var card = _cardRepo.Get(cardId);
            
            if (card == null)
            {
                return BadRequest("Card not found…");
            }
            
            _cardRepo.Delete(card);

            return "Card deleted…";
        }
    }
}

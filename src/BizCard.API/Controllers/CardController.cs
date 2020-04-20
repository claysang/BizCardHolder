using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CardController(IRepository<Card> cardRepo, IMapper mapper)
        {
            _cardRepo = cardRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CardInfo()
        {
            var cards = await _cardRepo.All().ToListAsync();
            var cardVms = _mapper.Map<List<Card>, List<CardViewModel>>(cards);
            return Ok(cardVms);
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> AddCard([FromBody] CardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var card = _mapper.Map<CardViewModel, Card>(model);

            await _cardRepo.SaveAsync(card);

            return CreatedAtAction(nameof(CardInfo), new { id = card.Id }, "Card created…");
        }
        
        [HttpPut]
        [Route("{cardId?}")]
        public async Task<ActionResult<object>> EditCard([FromBody] CardViewModel vm, int cardId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var card = _cardRepo.Get(cardId);
            
            if (card == null)
            {
                return BadRequest("Card not found…");
            }

            var vmProperties = typeof(CardViewModel).GetProperties();

            foreach (var property in vmProperties)
            {
                var propName = property.Name;
                var vmValue = property.GetValue(vm);
                
                typeof(Card).GetProperty(propName)?.SetValue(card, vmValue);
            }
            
            card.ModifiedAtUtc = DateTime.Now;

            await _cardRepo.UpdateAsync(card);

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

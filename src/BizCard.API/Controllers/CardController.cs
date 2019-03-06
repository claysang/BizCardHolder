using System;
using BizCard.Core.Data;
using BizCard.Core.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CardInfo()
        {
            var card = new Card
            {
                Name = "CS",
            };

            _cardRepo.Save(card);

            return Ok(new { id = card.Id, name = card.Name });
        }

    }
}

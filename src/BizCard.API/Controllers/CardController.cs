using System.Threading.Tasks;
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
    }
}

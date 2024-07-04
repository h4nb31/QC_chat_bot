using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualityControl.Models;

namespace QualityControl.Controllers
{
    [Route("TelegramBotViews")]
    public class TelegramBotController : Controller
    {
        private readonly ILogger<TelegramBotController> _logger;
        private readonly ApplicationContext? _db;
        private readonly CancellationTokenSource _cts = new();  //Токен отмены


        public TelegramBotController(ILogger<TelegramBotController> Logger, ApplicationContext? context)
        {
            _logger = Logger;
            _db = context;
        }

        //Вывод отзывов из базы
        [HttpGet("Display")]
        public async Task<IActionResult> Display()
        {

            ArgumentNullException.ThrowIfNull(_db);
            var reciew = await _db.ReviewListModels.ToListAsync();
            return View(reciew);
        }

        //Детальный просмотр отзыва
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ArgumentNullException.ThrowIfNull(_db);
            var rev = await _db.ReviewListModels.FindAsync(id);
            
            if(rev == null)
            {
                return NotFound();
            }

            return View(rev);
        }
    }
}

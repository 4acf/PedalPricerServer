using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Dto;
using PedalPricerServer.Models;
using PedalPricerServer.Services;
using System.Net.Mime;

namespace PedalPricerServer.Controllers
{
    [Route("pedals")]
    [ApiController]
    public class PedalsController : ControllerBase
    {
        private readonly PedalPricerDbContext _dbContext;
        private readonly FileService _fileService;

        public PedalsController(PedalPricerDbContext dbContext, FileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPedals()
        {
            return await _dbContext.Pedals.Select(pedal => new ItemDto(
                pedal.ID,
                pedal.Brand,
                pedal.Name
            )).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedal>> GetPedal(Guid id)
        {
            var pedal = await _dbContext.Pedals.FindAsync(id);

            if (pedal == null)
            {
                return NotFound();
            }

            return pedal;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPedalImage(Guid id)
        {
            var pedal = await _dbContext.Pedals.FindAsync(id);

            if (pedal == null)
            {
                return NotFound();
            }
            try
            {
                Stream stream = await _fileService.ReadImage(MediaFolder.Pedals, pedal.Name);

                Response.Headers.Append("Content-Disposition", new ContentDisposition
                {
                    FileName = $"{pedal.Name}.png",
                    Inline = true
                }.ToString());

                return File(stream, "image/png");
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }
        }

    }
}

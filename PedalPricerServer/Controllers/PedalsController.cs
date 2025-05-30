using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Db;
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

        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPedalInfo()
        {
            return await _dbContext.Pedals.Select(pedal => new ItemDto(
                pedal.ID,
                pedal.Brand,
                pedal.Name
            )).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedal>> GetPedal(string id)
        {
            var pedal = await _dbContext.Pedals.FindAsync(id);

            if (pedal == null)
            {
                return NotFound();
            }

            return pedal;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPedalImage(string id)
        {
            var pedal = await _dbContext.Pedals.FindAsync(id);

            if (pedal == null)
            {
                return NotFound();
            }
            try
            {
                Stream stream = await _fileService.ReadFile(MediaFolder.Pedals, pedal.Filename);

                Response.Headers.Append("Content-Disposition", new ContentDisposition
                {
                    FileName = pedal.Filename,
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

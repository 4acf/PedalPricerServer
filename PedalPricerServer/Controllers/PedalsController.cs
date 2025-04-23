using Microsoft.AspNetCore.Http.Features;
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

        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPedalInfo()
        {
            return await _dbContext.Pedals.Select(pedal => new ItemDto(
                pedal.ID,
                pedal.Brand,
                pedal.Name
            )).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedal>>> GetPedals(string rawIDs)
        {
            var ids = rawIDs.Split(',');
            var guids = new List<Guid>();

            foreach (var id in ids)
            {
                if (Guid.TryParse(id, out var guid))
                {
                    guids.Add(guid);
                }
            }

            var pedals = new List<Pedal>();

            foreach (var guid in guids)
            {
                var pedal = await _dbContext.Pedals.FindAsync(guid);
                if (pedal != null)
                {
                    pedals.Add(pedal);
                }
            }

            return pedals;
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
                Stream stream = await _fileService.ReadImage(MediaFolder.Pedals, pedal.Filename);

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

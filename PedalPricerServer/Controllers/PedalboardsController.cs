using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Dto;
using PedalPricerServer.Models;
using PedalPricerServer.Services;
using System.Net.Mime;

namespace PedalPricerServer.Controllers
{
    [Route("pedalboards")]
    [ApiController]
    public class PedalBoardsController : ControllerBase
    {
        private readonly PedalPricerDbContext _dbContext;
        private readonly FileService _fileService;

        public PedalBoardsController(PedalPricerDbContext dbContext, FileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPedalboardInfo()
        {
            return await _dbContext.Pedalboards.Select(pedalboard => new ItemDto(
                pedalboard.ID,
                pedalboard.Brand,
                pedalboard.Name
            )).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedalboard>>> GetPedalboards(string rawIDs)
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

            var pedalboards = new List<Pedalboard>();

            foreach (var guid in guids)
            {
                var pedalboard = await _dbContext.Pedalboards.FindAsync(guid);
                if (pedalboard != null)
                {
                    pedalboards.Add(pedalboard);
                }
            }

            return pedalboards;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPedalboardImage(Guid id)
        {
            var pedalboard = await _dbContext.Pedalboards.FindAsync(id);

            if (pedalboard == null)
            {
                return NotFound();
            }

            try
            {
                Stream stream = await _fileService.ReadImage(MediaFolder.Pedalboards, pedalboard.Filename);

                Response.Headers.Append("Content-Disposition", new ContentDisposition
                {
                    FileName = pedalboard.Filename,
                    Inline = true
                }.ToString());

                return File(stream, "image/png");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

    }
}

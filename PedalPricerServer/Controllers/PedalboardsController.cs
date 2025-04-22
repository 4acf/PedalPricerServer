using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Dto;
using PedalPricerServer.Models;
using PedalPricerServer.Services;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPedalboards()
        {
            return await _dbContext.Pedalboards.Select(pedalboard => new ItemDto(
                pedalboard.ID,
                pedalboard.Brand,
                pedalboard.Name
            )).ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedalboard>> GetPedalboard(Guid id)
        {
            var pedalboard = await _dbContext.Pedalboards.FindAsync(id);

            if (pedalboard == null)
            {
                return NotFound();
            }

            return pedalboard;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPedalboardImage(Guid id)
        {
            var pedalboard = await _dbContext.Pedalboards.FindAsync(id);

            if (pedalboard == null)
            {
                return NotFound();
            }

            Stream stream = await _fileService.ReadImage(MediaFolder.Pedalboards, pedalboard.Name);

            Response.Headers.Append("Content-Disposition", new ContentDisposition
            {
                FileName = $"{pedalboard.Name}.png",
                Inline = true
            }.ToString());

            return File(stream, "image/png");
        }

    }
}

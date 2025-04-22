using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Models;
using PedalPricerServer.Services;

namespace PedalPricerServer.Controllers
{
    [Route("pedalboards")]
    [ApiController]
    public class PedalBoardsController : ControllerBase
    {
        private readonly PedalPricerDbContext _context;
        private readonly FileService _fileService;

        public PedalBoardsController(PedalPricerDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedalboard>>> GetPedalboards()
        {
            return await _context.Pedalboards.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedalboard(Guid id)
        {
            var pedalboard = await _context.Pedalboards.FindAsync(id);

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

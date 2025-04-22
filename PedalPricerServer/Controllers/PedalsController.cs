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
    [Route("pedals")]
    [ApiController]
    public class PedalsController : ControllerBase
    {
        private readonly PedalPricerDbContext _context;
        private readonly FileService _fileService;

        public PedalsController(PedalPricerDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedal>>> GetPedals()
        {
            return await _context.Pedals.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedal(Guid id)
        {
            var pedal = await _context.Pedals.FindAsync(id);

            if (pedal == null)
            {
                return NotFound();
            }

            Stream stream = await _fileService.ReadImage(MediaFolder.Pedals, pedal.Name);

            Response.Headers.Append("Content-Disposition", new ContentDisposition
            {
                FileName = $"{pedal.Name}.png",
                Inline = true
            }.ToString());

            return File(stream, "image/png");
        }

    }
}

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
    [Route("powersupplies")]
    [ApiController]
    public class PowerSuppliesController : ControllerBase
    {
        private readonly PedalPricerDbContext _context;
        private readonly FileService _fileService;

        public PowerSuppliesController(PedalPricerDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PowerSupply>>> GetPowerSupplies()
        {
            return await _context.PowerSupplies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPowerSupply(Guid id)
        {
            var powerSupply = await _context.PowerSupplies.FindAsync(id);

            if (powerSupply == null)
            {
                return NotFound();
            }

            Stream stream = await _fileService.ReadImage(MediaFolder.PowerSupplies, powerSupply.Name);

            Response.Headers.Append("Content-Disposition", new ContentDisposition
            {
                FileName = $"{powerSupply.Name}.png",
                Inline = true
            }.ToString());

            return File(stream, "image/png");
        }

    }
}

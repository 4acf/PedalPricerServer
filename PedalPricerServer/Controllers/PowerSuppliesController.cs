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
    [Route("powersupplies")]
    [ApiController]
    public class PowerSuppliesController : ControllerBase
    {
        private readonly PedalPricerDbContext _dbContext;
        private readonly FileService _fileService;

        public PowerSuppliesController(PedalPricerDbContext dbContext, FileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPowerSupplies()
        {
            return await _dbContext.PowerSupplies.Select(powerSupply => new ItemDto(
                powerSupply.ID,
                powerSupply.Brand,
                powerSupply.Name
            )).ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PowerSupply>> GetPowerSupply(Guid id)
        {
            var powerSupply = await _dbContext.PowerSupplies.FindAsync(id);

            if (powerSupply == null)
            {
                return NotFound();
            }

            return powerSupply;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPowerSupplyImage(Guid id)
        {
            var powerSupply = await _dbContext.PowerSupplies.FindAsync(id);

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

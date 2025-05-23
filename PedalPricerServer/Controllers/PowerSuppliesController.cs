﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Dto;
using PedalPricerServer.Models;
using PedalPricerServer.Services;
using System.Net.Mime;

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

        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetPowerSupplyInfo()
        {
            return await _dbContext.PowerSupplies.Select(powerSupply => new ItemDto(
                powerSupply.ID,
                powerSupply.Brand,
                powerSupply.Name
            )).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PowerSupply>>> GetPowerSupplies(string rawIDs)
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

            var powerSupplies = new List<PowerSupply>();

            foreach (var guid in guids)
            {
                var powerSupply = await _dbContext.PowerSupplies.FindAsync(guid);
                if (powerSupply != null)
                {
                    powerSupplies.Add(powerSupply);
                }
            }

            return powerSupplies;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPowerSupplyImage(Guid id)
        {
            var powerSupply = await _dbContext.PowerSupplies.FindAsync(id);

            if (powerSupply == null)
            {
                return NotFound();
            }

            try
            {
                Stream stream = await _fileService.ReadImage(MediaFolder.PowerSupplies, powerSupply.Filename);

                Response.Headers.Append("Content-Disposition", new ContentDisposition
                {
                    FileName = powerSupply.Filename,
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

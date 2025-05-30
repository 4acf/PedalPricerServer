﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Db;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedalboard>> GetPedalboard(string id)
        {
            var pedalboard = await _dbContext.Pedalboards.FindAsync(id);

            if (pedalboard == null)
            {
                return NotFound();
            }

            return pedalboard;
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPedalboardImage(string id)
        {
            var pedalboard = await _dbContext.Pedalboards.FindAsync(id);

            if (pedalboard == null)
            {
                return NotFound();
            }

            try
            {
                Stream stream = await _fileService.ReadFile(MediaFolder.Pedalboards, pedalboard.Filename);

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

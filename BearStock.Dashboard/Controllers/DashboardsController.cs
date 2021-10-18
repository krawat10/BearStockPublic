using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearStock.Dashboard.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BearStock.Dashboard.Entities;
using BearStock.Dashboard.Helpers;
using BearStock.Dashboard.Services;
using BearStock.Tools.Exceptions;
using BearStock.Tools.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BearStock.Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly DashboardsService _service;
        private ILogger<DashboardsController> _logger;

        public DashboardsController(DashboardsService service, ILoggerFactory loggerFactory)
        {
            _service = service;
            _logger = loggerFactory.CreateLogger<DashboardsController>();
        }

        private string GetUserId()
        {
            return HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Id)?.Value;
        }


        // GET: api/Dashboards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entities.Dashboard>>> GetDashboards()
        {
            if (GetUserId().IsNullOrEmpty()) return Unauthorized();

            return Ok(await _service.GetDashboards(GetUserId()));
        }

        // GET: api/Dashboards/Default
        [HttpGet("{name}")]
        public async Task<ActionResult<Entities.Dashboard>> GetDashboard(string name)
        {
            if (GetUserId().IsNullOrEmpty()) return Unauthorized();

            try
            {
                return Ok(await _service.GetDashboard(name, GetUserId()));
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Dashboards/Default
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{name}")]
        public async Task<IActionResult> PutDashboard(string name, DashboardDTO dto)
        {
            if (name != dto.Name)
            {
                return BadRequest();
            }

            if (GetUserId().IsNullOrEmpty()) return Unauthorized();

            try
            {
                await _service.UpdateDashboard(dto, GetUserId());
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        // POST: api/Dashboards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entities.Dashboard>> PostDashboard(DashboardDTO dto)
        {
            if (GetUserId().IsNullOrEmpty()) return Unauthorized();

            try
            {
                await _service.AddDashboard(dto, GetUserId());
            }
            catch (ObjectDuplicateException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetDashboard", new {name = dto.Name}, dto);
        }

        // DELETE: api/Dashboards/Default
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteDashboard(string name)
        {
            if (GetUserId().IsNullOrEmpty()) return Unauthorized();

            try
            {
                await _service.DeleteDashboards(name, GetUserId());
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

    }
}
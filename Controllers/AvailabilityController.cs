using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using PomeloHealthApi.Database;
using PomeloHealthApi.Models;
using PomeloHealthApi.Validators;

namespace PomeloHealthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly ILogger<AvailabilityController> _logger;
        private readonly DatabaseContext _context;

        public AvailabilityController(ILogger<AvailabilityController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Clinic>> GetAll()
        {
            IEnumerable availabilities = _context.Availabilities.Include(i => i.Provider).ToList();
            if (availabilities == null)
            {
                return NotFound();
            }
            return Ok(availabilities);
        }

        [HttpGet("{availabilityId}")]
        public async Task<ActionResult<Availability>> Get(int availabilityId)
        {
            var availability = await _context.Availabilities.FindAsync(availabilityId);
            if (availability == null)
            {
                return NotFound();
            }
            return availability;
        }

        [HttpPost]
        public ActionResult<Availability> Post(string providerId, DateTime startDate, DateTime endDate)
        {
            Provider provider = _context.Providers.Find(providerId);
            if (provider == null)
            {
                return NotFound("Unable to create a new availability, incorrect provider.");
            }

            Availability availability = new Availability()
            {
                Provider = provider,
                StartDate = startDate,
                EndDate = endDate
            };

            Availability existingAvailability = _context.Availabilities.Where(r =>
                r.Provider == availability.Provider
                && (
                    (r.StartDate == availability.StartDate || r.EndDate == availability.EndDate)
                    || (availability.StartDate >= r.StartDate && availability.EndDate <= r.EndDate)
                    || (availability.StartDate > r.StartDate && availability.StartDate < r.EndDate)
                    || (availability.EndDate > r.StartDate && availability.EndDate < r.EndDate)
                )
            ).FirstOrDefault();

            if (existingAvailability is Availability || !DateValidator.IsValid(availability))
            {
                return Problem("Unable to create a new availability, please check your parameters.");
            }

            try
            {
                _context.Availabilities.Add(availability);
                _context.SaveChanges();
            } catch (Exception e)
            {
                if (e is MySqlException || e is DbUpdateException)
                {
                    return Problem("Unable to create a new Availability.");
                }
                throw;
            }
            return Ok(availability);
        }

        [HttpDelete("{availabilityId}")]
        public async Task<ActionResult> Delete(int availabilityId)
        {
            var availability = await _context.Availabilities.FindAsync(availabilityId);
            if (availability == null)
            {
                return NotFound();
            }
            _context.Availabilities.Remove(availability);
            _context.SaveChanges();
            return Ok();
        }
    }
}
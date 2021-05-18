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

namespace PomeloHealthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly ILogger<ClinicController> _logger;
        private readonly DatabaseContext _context;

        public ClinicController(ILogger<ClinicController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Clinic>> GetAll()
        {
            IEnumerable clinics = _context.Clinics.ToList();
            if (clinics == null)
            {
                return NotFound();
            }
            return Ok(clinics);
        }

        [HttpGet("{clinicId}")]
        public async Task<ActionResult<Clinic>> Get(string clinicId)
        {
            var clinic = await _context.Clinics.FindAsync(clinicId);
            if (clinic == null)
            {
                return NotFound();
            }
            return clinic;
        }

        [HttpPost]
        public ActionResult<Clinic> Post(string clinicId)
        {
            Clinic clinic = new Clinic() { Id = clinicId };
            try
            {
                _context.Clinics.Add(clinic);
                _context.SaveChanges();
            } catch (Exception e)
            {
                if (e is MySqlException || e is DbUpdateException)
                {
                    return Problem("Unable to create a new clinic");
                }
                throw;
            }
            return clinic;
        }

        [HttpDelete("{clinicId}")]
        public async Task<ActionResult> Delete(string clinicId)
        {
            var clinic = await _context.Clinics.FindAsync(clinicId);
            if (clinic == null)
            {
                return NotFound();
            }
            _context.Clinics.Remove(clinic);
            _context.SaveChanges();
            return Ok();
        }
    }
}
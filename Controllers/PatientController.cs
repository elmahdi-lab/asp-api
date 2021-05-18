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
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly DatabaseContext _context;

        public PatientController(ILogger<PatientController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Clinic>> GetAll()
        {
            IEnumerable patients = _context.Patients.ToList();
            if (patients == null)
            {
                return NotFound();
            }
            return Ok(patients);
        }

        [HttpGet("{PatientId}")]
        public async Task<ActionResult<Patient>> Get(string PatientId)
        {
            var patient = await _context.Patients.FindAsync(PatientId);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public ActionResult<Patient> Post(string firstname, string lastname)
        {
            Patient patient = new Patient()
            {
                Id = firstname + " " + lastname,
                Firstname = firstname,
                Lastname = lastname
            };
            try
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
            } catch (Exception e)
            {
                if (e is MySqlException || e is DbUpdateException)
                {
                    return Problem("Unable to create a new Patient.");
                }
                throw;
            }
            return Ok(patient);
        }

        [HttpDelete("{PatientId}")]
        public async Task<ActionResult> Delete(string PatientId)
        {
            var patient = await _context.Patients.FindAsync(PatientId);
            if (patient == null)
            {
                return NotFound();
            }
            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return Ok();
        }
    }
}